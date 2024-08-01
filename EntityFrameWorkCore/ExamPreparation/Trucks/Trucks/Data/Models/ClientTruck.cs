﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Data.Models
{
    public class ClientTruck
    {
        [Key]
        [ForeignKey(nameof(ClientId))]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        [Key]
        [ForeignKey(nameof(TruckId))]
        public int TruckId { get; set; }

        public virtual Truck Truck { get; set; }
    }
}
