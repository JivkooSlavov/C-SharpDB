namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Net;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder sb = new StringBuilder();

            var actualDistricts = dbContext.Districts
                .Select(x => x.Name);

            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Districts";
            ICollection<District> districtToImport = new List<District>();

            ImportDistrictsXmlDto[] deserializeDistricts = xmlHelper.Deserialize<ImportDistrictsXmlDto[]>(xmlDocument, xmlRoot);
            foreach (var dis in deserializeDistricts)
            {
                if (!IsValid(dis))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (actualDistricts.Contains(dis.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                District district = new District()
                {
                    Name = dis.Name,
                    PostalCode = dis.PostalCode,
                    Region = (Region)Enum.Parse(typeof(Region), dis.Region)
                };
                foreach (var prop in dis.Properties)
                {
                    if (!IsValid(prop))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime acquisitionDate = DateTime
                        .ParseExact(prop.DateOfAcquisition, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

                    if (dbContext.Properties.Any(p => p.PropertyIdentifier == prop.PropertyIdentifier) || district.Properties.Any(dp => dp.PropertyIdentifier == prop.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (dbContext.Properties.Any(p => p.Address == prop.Address) || district.Properties.Any(dp => dp.Address == prop.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Property property = new Property()
                    {
                        PropertyIdentifier = prop.PropertyIdentifier,
                        Area = (int)prop.Area,
                        Details = prop.Details,
                        Address= prop.Address,
                        DateOfAcquisition = acquisitionDate
                    };

                    district.Properties.Add(property);
                }
                districtToImport.Add(district);
                sb.AppendLine(String.Format(SuccessfullyImportedDistrict, district.Name, district.Properties.Count()));
            }
            dbContext.Districts.AddRange(districtToImport);
            dbContext.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            ImportCitizensJsonDto[] citizenDtos = JsonConvert.DeserializeObject<ImportCitizensJsonDto[]>(jsonDocument);
            ICollection<Citizen> citizens = new List<Citizen>();

            foreach (var citDto in citizenDtos!)
            {
                if (!IsValid(citDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (citDto.MaritalStatus!= "Unmarried" && citDto.MaritalStatus != "Married" && citDto.MaritalStatus != "Divorced" && citDto.MaritalStatus != "Widowed")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime dateOfBirth = DateTime
                    .ParseExact(citDto.BirthDate, "dd-MM-yyyy", CultureInfo
                    .InvariantCulture, DateTimeStyles.None);

                Citizen citizen = new Citizen()
                {
                    FirstName = citDto.FirstName,
                    LastName = citDto.LastName,
                    BirthDate = dateOfBirth,
                    MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), citDto.MaritalStatus)
                };

                foreach (var propId in citDto.Properties)
                {
                    PropertyCitizen propertyCitizen = new PropertyCitizen()
                    {
                        Citizen = citizen,
                        PropertyId = propId
                    };
                    citizen.PropertiesCitizens.Add(propertyCitizen);
                }
                citizens.Add(citizen);
                sb.AppendLine(string
                    .Format(SuccessfullyImportedCitizen, citizen.FirstName, citizen.LastName, citizen.PropertiesCitizens.Count));
            }
            dbContext.Citizens.AddRange(citizens);
            dbContext.SaveChanges();

            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
