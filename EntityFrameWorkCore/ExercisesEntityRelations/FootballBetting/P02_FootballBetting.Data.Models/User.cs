using P02_FootballBetting.Common;
using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
                Bets = new HashSet<Bet>();
        }

        [Key]
        public int UserId { get; set; }


        [MaxLength(ValidationConstrants.UserNameMaxLength)]
        public string Username { get; set; } = null!;

        [MaxLength(ValidationConstrants.NameMaxLength)]
        public string Name { get; set; }


        [MaxLength(ValidationConstrants.PasswordMaxLength)]
        public string Password { get; set; }

        [MaxLength(ValidationConstrants.EmailMaxLength)]
        public string Email { get; set; }

        public decimal Balance {  get; set; }

        public virtual ICollection<Bet> Bets { get; set;}
    }
}
