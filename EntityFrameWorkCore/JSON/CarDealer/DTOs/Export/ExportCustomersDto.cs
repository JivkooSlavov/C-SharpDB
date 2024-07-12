using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Export
{
    public class ExportCustomersDto
    {
        public string Name { get; set; } = null!;

        public DateTime BirthDate { get; set; } 

        public bool IsYoungDriver { get; set; }
    }
}
