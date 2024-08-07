namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Text.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPatientsJsonDto[] patientsDto = JsonConvert.DeserializeObject<ImportPatientsJsonDto[]>(jsonString);
            ICollection<Patient> patients = new List<Patient>();

            foreach (var pat in patientsDto)
            {
                if (!IsValid(pat))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new Patient()
                {
                    FullName = pat.FullName,
                    AgeGroup=(AgeGroup)pat.AgeGroup,
                    Gender =(Gender)pat.Gender
                };

                foreach (var medId in pat.Medicines)
                {
                    if (patient.PatientsMedicines.Any(x=>x.MedicineId==medId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine patientMedicine = new PatientMedicine()
                    {
                        Patient = patient,
                        MedicineId = medId
                    };

                    patient.PatientsMedicines.Add(patientMedicine);
                }
                patients.Add(patient);
                sb.AppendLine(String.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count()));
            }
            context.AddRange(patients);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();


            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Pharmacies";

            ICollection<Pharmacy> validPharmacies = new List<Pharmacy>();

            ImportPharmaciesXmlDto[] pharmacyDtos = xmlHelper.Deserialize<ImportPharmaciesXmlDto[]>(xmlString, xmlRoot);

            foreach (var pharmacyDto in pharmacyDtos)
            {
                if (!IsValid(pharmacyDto) ||
                    !bool.TryParse(pharmacyDto.IsNonStop, out bool result))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    IsNonStop = result,
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber
                };

                foreach (var medicineDto in pharmacyDto.Medicines)
                {
                    if (!IsValid(medicineDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (DateTime.ParseExact(medicineDto.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        <= DateTime.ParseExact(medicineDto.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        || pharmacy.Medicines.Any(m => m.Name == medicineDto.Name && m.Producer == medicineDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicine = new Medicine()
                    {
                        Category = (Category)medicineDto.Category,
                        Name = medicineDto.Name,
                        Price = (decimal)medicineDto.Price,
                        ProductionDate = DateTime.ParseExact(medicineDto.ProductionDate, "yyyy-MM-dd",
                            CultureInfo.InvariantCulture),
                        ExpiryDate = DateTime.ParseExact(medicineDto.ExpiryDate, "yyyy-MM-dd",
                            CultureInfo.InvariantCulture),
                        Producer = medicineDto.Producer
                    };

                    pharmacy.Medicines.Add(medicine);
                }

                validPharmacies.Add(pharmacy);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));
            }

            context.Pharmacies.AddRange(validPharmacies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
