using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cadastre.Data.Constrain;

namespace Cadastre.DataProcessor.ImportDtos
{
    public class ImportCitizensJsonDto
    {
        [Required]
        [MinLength(CitizenFirstNameMinLength)]
        [MaxLength(CitizenFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(CitizenLastNameMinLength)]
        [MaxLength(CitizenLastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]

        public string MaritalStatus { get; set; }

        public int [] Properties { get; set; }
    }
}
