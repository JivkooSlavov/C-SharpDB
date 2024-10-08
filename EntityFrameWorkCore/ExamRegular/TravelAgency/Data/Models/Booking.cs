﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Data.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [ForeignKey(nameof(CustomerId))]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Required]
        [ForeignKey(nameof(TourPackageId))]
        public int TourPackageId { get; set; }

        public virtual TourPackage TourPackage {  get; set; }


    }
}
