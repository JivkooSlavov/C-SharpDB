using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TravelAgency.Data.Constraints;

namespace TravelAgency.DataProcessor.ImportDtos
{
    public class ImportBookingsJsonDto
    {

        [Required]
        public string BookingDate { get; set; }

        [Required]
        [MinLength(CustomerFullNameMinLength)]
        [MaxLength(CustomerFullNameMaxLength)]
        public string CustomerName {  get; set; }

        [Required]
        [MinLength(TourPackageNameMinLength)]
        [MaxLength(TourPackageNameMaxLength)]
        public string TourPackageName {  get; set; }
    }
}
