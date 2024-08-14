using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cadastre.Data.Constrain;

namespace Cadastre.Data.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(PropertyIndetifierMaxLength)]
        public string PropertyIdentifier {get; set; }

        [Required]
        public int Area { get; set; }

        [MaxLength(PropertyDetailsMaxLength)]
        public string Details { get; set; }
        [Required]
        [MaxLength(PropertyAddressMaxLength)]
        public string Address { get; set; }

        [Required]
        public DateTime DateOfAcquisition { get; set; }
        
        [Required]
        [ForeignKey(nameof(DistrictId))]
        public int DistrictId { get; set; }

        public virtual District District {get; set; }

        public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new HashSet<PropertyCitizen>();
    }
}
