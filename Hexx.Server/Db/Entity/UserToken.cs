using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexx.Server.Db.Entity
{
    [Table("UserTokens")]
    public class UserToken : BaseEntity<UserToken>
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("ID")]
        public User User { get; set; }

        [Required]
        [MaxLength(64)]
        [MinLength(64)]
        public string Token { get; set; }

        [Required]
        [MaxLength(32)]
        public string ClientGuid { get; set; }
    }
}
