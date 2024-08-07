using Cadastre.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Cadastre.Data.Constrain;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType(nameof(Property))]
    public class ImportPropertiesDtoXml
    {
        [XmlElement(nameof(PropertyIdentifier))]
        [Required]
        [MinLength(PropertyIdentifierMinLength)]
        [MaxLength(PropertyIndetifierMaxLength)]
        public string PropertyIdentifier { get; set; }

        [XmlElement(nameof(Area))]
        [Required]
        public uint Area { get; set; }

        [MinLength(PropertyDetailsMinLength)]
        [MaxLength(PropertyDetailsMaxLength)]
        [XmlElement(nameof(Details))]
        public string? Details { get; set; }

        [Required]
        [MinLength(PropertyDetailsMinLength)]
        [MaxLength(PropertyAddressMaxLength)]
        [XmlElement(nameof(Address))]
        public string Address { get; set; }

        [Required]
        [XmlElement(nameof(DateOfAcquisition))]
        public string DateOfAcquisition { get; set; }
    }
}
