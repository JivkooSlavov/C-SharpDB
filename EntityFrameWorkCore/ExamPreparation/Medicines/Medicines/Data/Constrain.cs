using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data
{
    public static class Constrain
    {
        //Pharmacy
        public const byte PharmacyNameMinLength = 2;
        public const byte PharmacyNameMaxLength = 50;

        //Patient
        public const byte PatientFullNameMinLength = 5;
        public const byte PatientFullNameMaxLength = 100;


        //Medicine
        public const byte MedicineNameMinLength = 3;
        public const byte MedicineNameMaxLength = 150;

        public const string MedicinePriceMinLength = "0.01";
        public const string MedicinePriceMaxLength = "1000.00";

        public const byte MedicineProducerMinLength = 3;
        public const byte MedicineProducerMaxLength = 100;

        public const int AgeGroupMinLength = (int)AgeGroup.Child;
        public const int AgeGroupMaxLength = (int)AgeGroup.Senior;

        public const int CategoryMinLength = (int)Category.Analgesic;
        public const int CategoryMaxLength = (int)Category.Vaccine;

        public const int GenderMinLength = (int)Gender.Male;
        public const int GenderMaxLength = (int)Gender.Female;
    }
}
