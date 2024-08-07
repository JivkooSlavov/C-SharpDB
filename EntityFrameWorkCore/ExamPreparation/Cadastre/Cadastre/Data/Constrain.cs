using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data
{
    public static class Constrain
    {
        //DIstrict
        public const byte DistrictNameMinLength = 2;
        public const byte DistrictNameMaxLength = 80;

        //Citize

        public const byte CitizenFirstNameMinLength = 2;
        public const byte CitizenFirstNameMaxLength = 30;
        public const byte CitizenLastNameMinLength = 2;
        public const byte CitizenLastNameMaxLength = 30;

        //Property
        public const byte PropertyIdentifierMinLength = 16;
        public const byte PropertyIndetifierMaxLength = 20;

        public const int PropertyDetailsMinLength = 5;
        public const int PropertyDetailsMaxLength = 500;

        public const int PropertyAddressMinLength = 5;
        public const int PropertyAddressMaxLength = 200;

    }
}
