using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Trucks.Data.Constraints;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(DespatcherNameMaxLength)]
        public string Name { get; set; }

        public string? Position { get; set; }

        public virtual ICollection<Truck> Trucks { get; set; } = new HashSet<Truck>();
    }
}
