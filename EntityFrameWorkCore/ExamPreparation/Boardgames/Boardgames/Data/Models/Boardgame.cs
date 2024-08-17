using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boardgames.Data.Constraints;

namespace Boardgames.Data.Models
{
    public class Boardgame
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(BoardgameNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public double Rating  { get; set; }

        [Required]
        [MaxLength(BoardgameYearMaxLength)]
        public int YearPublished { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public string Mechanics { get; set; }

        [Required]
        [ForeignKey(nameof(CreatorId))]
        public int CreatorId {  get; set; }

        public virtual Creator Creator { get; set; }

        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; } = new HashSet<BoardgameSeller>();

    }
}
