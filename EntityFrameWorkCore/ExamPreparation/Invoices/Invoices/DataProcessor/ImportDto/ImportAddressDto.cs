using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Invoices.Data.Constaints;


namespace Invoices.DataProcessor.ImportDto
{
    [XmlType(nameof(Address))]
    public class ImportAddressDto
    {

        [XmlElement(nameof(StreetName))]
        [Required]
        [MinLength(AddressStreetNameMinLength)]
        [MaxLength(AddressStreetNameMaxLength)]
        public string StreetName { get; set; }

        [Required]
        [XmlElement(nameof(StreetNumber))]
        public int StreetNumber { get; set; }


        [XmlElement(nameof(PostCode))]
        [Required]
        public string PostCode { get; set; }

        [Required]
        [XmlElement(nameof(City))]
        [MinLength(AddressCityMinLength)]
        [MaxLength(AddressCityMaxLength)]
        public string City { get; set; }

        [XmlElement(nameof(Country))]
        [Required]
        [MinLength(AddressCountryMinLength)]
        [MaxLength(AddressCountryMaxLength)]
        public string Country { get; set; }
    }
}
