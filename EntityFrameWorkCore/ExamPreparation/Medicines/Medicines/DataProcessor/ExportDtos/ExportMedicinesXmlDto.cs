using Medicines.Data.Models;
using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType(nameof(Medicine))]
    public class ExportMedicinesXmlDto
    {
        [XmlAttribute(nameof(Category))]
        public string Category { get; set; }

        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlElement(nameof(Price))]
        public string Price { get; set; }

        [XmlElement(nameof(Producer))]
        public string Producer { get; set; }

        [XmlElement(nameof(BestBefore))]
        public string BestBefore { get; set; }

    }
}
