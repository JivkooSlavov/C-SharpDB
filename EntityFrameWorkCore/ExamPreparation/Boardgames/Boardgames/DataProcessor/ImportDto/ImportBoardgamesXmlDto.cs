using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
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
    [XmlType(nameof(Boardgame))]
    public class ImportBoardgamesXmlDto
    {
        [Required]
        [XmlElement(nameof(Name))]
        [MinLength(BoardgameNameMinLength)]
        [MaxLength(BoardgameNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [Range(BoardgameRatingMinLength, BoardgameRatingMaxLength)]
        [XmlElement(nameof(Rating))]
        public double Rating { get; set; }

        [Required]
        [XmlElement(nameof(YearPublished))]
        [Range(BoardgameYearMinLength, BoardgameYearMaxLength)]
        public int YearPublished { get; set; }

        [Required]
        [XmlElement(nameof(CategoryType))]
        [Range(BoardgameCategoryTypeMinValue, BoardgameCategoryTypeMaxValue)]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement(nameof(Mechanics))]
        public string Mechanics { get; set; }
    }
}
