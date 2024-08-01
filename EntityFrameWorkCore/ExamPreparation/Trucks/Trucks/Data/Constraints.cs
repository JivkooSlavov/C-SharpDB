using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models.Enums;

namespace Trucks.Data
{
    public static class Constraints
    {
        //Despatcher
        public const byte DespatcherNameMinLength = 2;
        public const byte DespatcherNameMaxLength = 40;

        //Client
        public const byte ClientNameMinLength = 3;
        public const byte ClientNameMaxLength = 40;

        public const byte ClientNationalityMinLengt = 2;
        public const byte ClientNationalityMaxLengt = 40;


        //Truck

        public const int TruckTankMinLength = 950;
        public const int TruckTankMaxLength = 1420;

        public const int TruckCargoMinLength = 5000;
        public const int TruckCargoMaxLength = 29000;

        public const byte TruckVinNumberLength = 17;

        public const byte TruckCategoryMin = (int)CategoryType.Flatbed;
        public const byte TruckCategoryMax = (int)CategoryType.Semi;

        public const byte TruckMakeMin = (int)MakeType.Daf;
        public const byte TruckMakeMax = (int)MakeType.Volvo;
    }
}
