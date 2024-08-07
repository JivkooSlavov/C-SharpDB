using Medicines.Data.Models;
using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Medicines.Data.Constrain;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType(nameof(Medicine))]
    public class ImportMedicinesXmlDto
    {
        [Required]
        [MinLength(MedicineNameMinLength)]
        [MaxLength(MedicineNameMaxLength)]
        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), MedicinePriceMinLength, MedicinePriceMaxLength)]
        [XmlElement(nameof(Price))]
        public decimal Price { get; set; }

        [Required]
        [XmlElement(nameof(ProductionDate))]
        public string ProductionDate { get; set; }
        [Required]
        [XmlElement(nameof(ExpiryDate))]
        public string ExpiryDate { get; set; }

        [Required]
        [MinLength(MedicineProducerMinLength)]
        [MaxLength(MedicineProducerMaxLength)]
        public string Producer { get; set; }

        [Required]
        [XmlAttribute("category")]
        [Range(CategoryMinLength, CategoryMaxLength)]
        public int Category { get; set; }
    }
}
