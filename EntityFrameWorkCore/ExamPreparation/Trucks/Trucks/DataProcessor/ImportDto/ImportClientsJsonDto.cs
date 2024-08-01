using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Trucks.Data.Constraints;

namespace Trucks.DataProcessor.ImportDto
{
    public class ImportClientsJsonDto
    {
        [Required]
        [MinLength(ClientNameMinLength)]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ClientNationalityMaxLengt)]
        [MinLength(ClientNationalityMinLengt)]
        public string Nationality { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]

        public int[] Trucks { get; set; }
    }
}
