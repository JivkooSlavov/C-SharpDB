using P02_FootballBetting.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            HomeGames = new HashSet<Game>();
            AwayGames = new HashSet<Game>();
            Players = new HashSet<Player>();
        }

        [Key]
        public int TeamId { get; set; }


        [MaxLength(ValidationConstrants.TeamNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(ValidationConstrants.LogoUrlMax)]
        public string LogoUrl { get; set; }

        [MaxLength(ValidationConstrants.TeamInitialMaxLength)]
        public string Initials { get; set; }

        public decimal Budget {  get; set; }

        [ForeignKey(nameof(PrimaryKitColor))]
        public int PrimaryKitColorId { get; set; }
        public virtual Color PrimaryKitColor { get; set; }

        [ForeignKey(nameof(SecondaryKitColor))]
        public int SecondaryKitColorId { get; set; }
        public virtual Color SecondaryKitColor { get; set; }

        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public virtual ICollection<Game> HomeGames { get; set; }
        public virtual ICollection<Game> AwayGames {  get; set; }
        public virtual ICollection<Player> Players { get; set; }

    }
}
