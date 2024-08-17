using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Boardgames.Data.Constraints;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType(nameof(Creator))]
    public class ImportCreatorsXmlDtop
    {
        [Required]
        [MinLength(CreatorFirstNameMinLength)]
        [MaxLength(CreatorFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(CreatorLastNameMinLength)]
        [MaxLength(CreatorLastNameMaxLength)]
        public string LastName { get; set; }

        [XmlArray(nameof(Boardgames))]
        public ImportBoardgamesXmlDto[] Boardgames { get; set; }
    }
}
