using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportClientsJsonDto
    {
        public string Name { get; set; }

        public ExportTrucksJsonDto[] Trucks {  get; set; }
    }
}
