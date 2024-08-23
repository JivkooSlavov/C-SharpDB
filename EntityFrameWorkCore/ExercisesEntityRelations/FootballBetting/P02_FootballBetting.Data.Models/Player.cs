using P02_FootballBetting.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        public Player()
        {
              PlayersStatistics = new HashSet<PlayerStatistic>();
        }
        [Key]
        public int PlayerId { get; set; }

        [MaxLength(ValidationConstrants.PlayerNameMaxLength)]
        public string Name { get; set; }

        public byte SquadNumber { get; set; }

        public bool IsInjured { get; set; }



        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }


        [ForeignKey(nameof(Team))]
        public int TeamId {  get; set; }
        
        public virtual Team Team { get; set; }


        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }
    }
}
