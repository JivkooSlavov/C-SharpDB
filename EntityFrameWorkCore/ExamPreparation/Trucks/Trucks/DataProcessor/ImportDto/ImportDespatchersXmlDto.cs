using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Data.Models;
using static Trucks.Data.Constraints;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType(nameof(Despatcher))]
    public class ImportDespatchersXmlDto
    {
        [Required]
        [MinLength(DespatcherNameMinLength)]
        [MaxLength(DespatcherNameMaxLength)]
        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlElement(nameof(Position))]
        public string Position { get; set; }


        [XmlArray(nameof(Trucks))]
        public ImportTrucksXmlDto[] Trucks {  get; set; }
    }
}
