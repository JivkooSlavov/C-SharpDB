﻿using Cadastre.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cadastre.Data.Constrain;

namespace Cadastre.Data.Models
{
    public class Citizen
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CitizenFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(CitizenLastNameMaxLength)]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public MaritalStatus MaritalStatus { get; set; }
        public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new HashSet<PropertyCitizen>();
    }
}
