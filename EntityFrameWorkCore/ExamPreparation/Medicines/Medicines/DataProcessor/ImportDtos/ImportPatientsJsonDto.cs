using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Medicines.Data.Constrain;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientsJsonDto
    {
        [Required]
        [MinLength(PatientFullNameMinLength)]
        [MaxLength(PatientFullNameMaxLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [Range(AgeGroupMinLength, AgeGroupMaxLength)]
        public int AgeGroup { get; set; }

        [Required]
        [Range(GenderMinLength, GenderMaxLength)]
        public int Gender { get; set; }

        [Required]
        public int [] Medicines {  get; set; }
    }
}
