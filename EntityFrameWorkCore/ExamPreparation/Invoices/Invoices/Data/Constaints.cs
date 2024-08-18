using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data
{
    public static class Constaints
    {
        //Product
        public const byte ProductNameMinLength = 9;
        public const byte ProductNameMaxLength = 30;

        public const string ProductPriceMinValue = "5.00";
        public const string ProductPriceMaxValue = "1000.00";

        public const byte ProductCategoryTyreMinValue = (int)CategoryType.ADR;
        public const byte ProductCategoryTyreMaxValue = (int)CategoryType.Tyres;

        //Client

        public const byte ClientNameMinLength = 10;
        public const byte ClientNameMaxLength = 25;
        public const byte ClientNumberVatMinLength = 10;
        public const byte ClientNumberVatMaxLength = 15;

        //Address

        public const byte AddressStreetNameMinLength = 10;
        public const byte AddressStreetNameMaxLength = 20;
        public const byte AddressCityMinLength = 5;
        public const byte AddressCityMaxLength = 15;
        public const byte AddressCountryMinLength = 5;
        public const byte AddressCountryMaxLength = 15;

        //Invoice

        public const int InvoiceNumberMinLength = 1_000_000_000;
        public const int InvoiceNumberMaxLength = 1_500_000_000;
        public const byte InvoiceMinCurrencyType = (int)CurrencyType.BGN;
        public const byte InvoiceMaxCurrencyType = (int)CurrencyType.USD;
    }
}
