using Medicines.Data.Models;
using Medicines.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Medicines.Data.Constrain;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType(nameof(Pharmacy))]
    public class ImportPharmaciesXmlDto
    {
        [Required]
        [MinLength(PharmacyNameMinLength)]
        [MaxLength(PharmacyNameMaxLength)]
        [XmlElement(nameof(Name))]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$")]
        [XmlElement(nameof(PhoneNumber))]
        public string PhoneNumber { get; set; }

        [XmlAttribute("non-stop")]
        [Required]
        [RegularExpression(@"^(true|false)$")]
        public string IsNonStop { get; set; }

        [XmlArray(nameof(Medicines))]
        public ImportMedicinesXmlDto[] Medicines { get; set; }
    }
}
