using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models.Enums;
using static Trucks.Data.Constraints;

namespace Trucks.Data.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public string VinNumber { get; set; }

        [MaxLength(TruckTankMaxLength)]
        public int TankCapacity { get; set; }

        [MaxLength(TruckCargoMaxLength)]
        public int CargoCapacity { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public MakeType MakeType { get; set; }

        [Required]
        [ForeignKey(nameof(DespatcherId))]
        public int DespatcherId { get; set; }

        public virtual Despatcher Despatcher { get; set; }

        public virtual ICollection<ClientTruck> ClientsTrucks {  get; set; } = new HashSet<ClientTruck>();

    }
}
