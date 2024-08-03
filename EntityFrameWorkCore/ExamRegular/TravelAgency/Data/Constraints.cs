using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Data
{
    public static class Constraints
    {
        //Customer

        public const byte CustomerFullNameMinLength = 4;
        public const byte CustomerFullNameMaxLength = 60;

        public const byte CustomerEmailMinLength = 6;
        public const byte CUstomerEMailMaxLength = 50;

        public const byte CustomerPhoneNumberLength = 13;
        

        //Guide

        public const byte GuideFullNameMinLength = 4;
        public const byte GuideFullNameMaxLength = 60;

        //TourPackage

        public const byte TourPackageNameMinLength = 2;
        public const byte TourPackageNameMaxLength = 40;

        public const int TourPackageDescriptionMaxLength = 200;

        public const string TourPackagePriceMinValue = "0.00";
    }
}
