using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TravelAgency.Data.Models;
using static TravelAgency.Data.Constraints;

namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType(nameof(Customer))]
    public class ImportCustomersXml
    {
        [XmlElement(nameof(FullName))]
        [Required]
        [MinLength(CustomerFullNameMinLength)]
        [MaxLength(CustomerFullNameMaxLength)]
        public string FullName { get; set; }

        [XmlElement(nameof(Email))]
        [Required]
        [MinLength(CustomerEmailMinLength)]
        [MaxLength(CUstomerEMailMaxLength)]
        public string Email { get; set; }

        [XmlAttribute(nameof(phoneNumber))]
        [Required]
        [StringLength(CustomerPhoneNumberLength)]
        [RegularExpression(@"^\+\d{12}$")]
        public string phoneNumber { get; set; }
    }
}
