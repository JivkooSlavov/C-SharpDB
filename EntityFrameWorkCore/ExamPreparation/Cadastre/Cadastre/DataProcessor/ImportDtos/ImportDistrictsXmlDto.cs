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
    [XmlType(nameof(District))]
    public class ImportDistrictsXmlDto
    {
        [XmlElement(nameof(Name))]

        [Required]
        [MinLength(DistrictNameMinLength)]
        [MaxLength(DistrictNameMaxLength)]
        public string Name { get; set; }

        [XmlElement(nameof(PostalCode))]
        [Required]
        [RegularExpression(@"^[A-Z]{2}-\d{5}$")]
        public string PostalCode { get; set; }

        [Required]
        [XmlAttribute(nameof(Region))]
        public string Region {  get; set; }


        [XmlArray(nameof(Properties))]
        public ImportPropertiesDtoXml[] Properties {  get; set; }
    }
}
