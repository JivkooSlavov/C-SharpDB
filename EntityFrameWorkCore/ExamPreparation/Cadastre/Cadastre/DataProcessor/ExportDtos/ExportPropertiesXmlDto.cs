using Cadastre.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Cadastre.Data.Constrain;

namespace Cadastre.DataProcessor.ExportDtos
{
    [XmlType(nameof(Property))]
    public class ExportPropertiesXmlDto
    {
        [XmlElement(nameof(PropertyIdentifier))]
        public string PropertyIdentifier { get; set; }

        [XmlElement(nameof(Area))]
        public int Area { get; set; }


        [XmlElement(nameof(DateOfAcquisition))]
        public string DateOfAcquisition { get; set; }

        [XmlAttribute("postal-code")]
        public string PostalCode { get; set; }
    }
}
