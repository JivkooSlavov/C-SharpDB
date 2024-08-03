using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TravelAgency.Data.Constraints;

namespace TravelAgency.Data.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CustomerFullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(CUstomerEMailMaxLength)]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

    }
}
