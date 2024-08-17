using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data
{
    public static class Constraints
    {
        //Seller
        public const byte SellerNameMinLength = 5;

        public const byte SellerNameMaxLength = 20;

        public const byte SellerAddressMinLength = 2;

        public const byte SellerAddressMaxLength = 30;

        //Creator

        public const byte CreatorFirstNameMinLength = 2;
        public const byte CreatorFirstNameMaxLength = 7;

        public const byte CreatorLastNameMinLength = 2;
        public const byte CreatorLastNameMaxLength = 7;

        //Boardgame

        public const byte BoardgameNameMinLength = 10;
        public const byte BoardgameNameMaxLength = 20;

        public const double BoardgameRatingMinLength = 1.00;
        public const double BoardgameRatingMaxLength = 10.00;

        public const int BoardgameYearMinLength = 2018;
        public const int BoardgameYearMaxLength = 2023;

        public const byte BoardgameCategoryTypeMinValue = (int)CategoryType.Abstract;
        public const byte BoardgameCategoryTypeMaxValue = (int)CategoryType.Strategy;
    }
}
