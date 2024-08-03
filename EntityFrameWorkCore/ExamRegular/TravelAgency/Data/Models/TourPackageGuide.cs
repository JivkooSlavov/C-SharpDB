using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Data.Models
{
    public class TourPackageGuide
    {
        [Key]
        [ForeignKey(nameof(TourPackageId))]
        public int TourPackageId { get; set; }

        public virtual TourPackage TourPackage { get; set; }

        [Key]
        [ForeignKey(nameof(GuideId))]
        public int GuideId { get; set; }

        public virtual Guide Guide { get; set; }
    }
}
