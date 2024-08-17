using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boardgames.Data.Constraints;

namespace Boardgames.DataProcessor.ExportDto
{
    public class ExportSellersJsonDto
    {
        public string Name { get; set; }
        public string Website { get; set; }

        public ExportBoardgameJsonDto[] Boardgames { get; set; }
    }
}
