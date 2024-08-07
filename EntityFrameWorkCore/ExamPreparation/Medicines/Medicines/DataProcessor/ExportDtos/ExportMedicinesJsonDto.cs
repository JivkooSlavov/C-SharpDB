using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ExportDtos
{
    public class ExportMedicinesJsonDto
    {
        public string Name { get; set; }
        public string Price { get; set; }

        public ExportPharmacyDto Pharmacy { get; set; }
    }
}
