using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Invoices.Data.Constaints;


namespace Invoices.DataProcessor.ExportDto
{
    public class ExmportProductDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public ExportProductClientsDto[] Clients {  get; set; } 
    }
}
