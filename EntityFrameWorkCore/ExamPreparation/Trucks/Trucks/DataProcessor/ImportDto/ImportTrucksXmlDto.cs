using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Data.Models;
using Trucks.Data.Models.Enums;
using static Trucks.Data.Constraints;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType(nameof(Truck))]
    public class ImportTrucksXmlDto
    {
        [XmlElement(nameof(RegistrationNumber))]
        [RegularExpression(@"^[A-Z]{2}\d{4}[A-Z]{2}$")]
        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        [StringLength(TruckVinNumberLength)]
        [XmlElement(nameof(VinNumber))]
        public string VinNumber { get; set; }

        [XmlElement(nameof(TankCapacity))]
        [Range(TruckTankMinLength, TruckTankMaxLength)]
        public int TankCapacity { get; set; }

        [XmlElement(nameof(CargoCapacity))]
        [Range(TruckCargoMinLength, TruckCargoMaxLength)]
        public int CargoCapacity { get; set; }

        [Required]
        [Range(TruckCategoryMin, TruckCategoryMax)]
        [XmlElement(nameof(CategoryType))]
        public int CategoryType { get; set; }

        [Required]
        [Range(TruckMakeMin, TruckMakeMax)]
        [XmlElement(nameof(MakeType))]
        public int MakeType { get; set; }
    }
}
