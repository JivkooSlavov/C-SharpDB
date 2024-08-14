using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    public class PropertyCitizen
    {
        [Key]
        [ForeignKey(nameof(PropertyId))]
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }

        [Key]
        [ForeignKey(nameof(CitizenId))]
        public int CitizenId { get; set; }

        public virtual Citizen Citizen { get; set; }
    }
}
