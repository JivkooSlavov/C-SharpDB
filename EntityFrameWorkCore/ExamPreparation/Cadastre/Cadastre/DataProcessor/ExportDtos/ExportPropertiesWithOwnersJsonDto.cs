using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportPropertiesWithOwnersJsonDto
    {
        public string PropertyIdentifier { get; set; }

        public int Area { get; set; }
        public string Address { get; set; }
        public string DateOfAcquisition { get; set; }

        public ExportOwnersJsonDtop[] Owners { get; set; }
    }
}
