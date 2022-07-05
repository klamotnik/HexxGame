using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexx.Server.Db.Entity
{
    [Table("Users")]
    public class User : BaseEntity<User>
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(64)]
        public byte[] Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string Mail { get; set; }
        
        [ForeignKey("ID")]
        public virtual UserData UserData { get; set; }

        [ForeignKey("ID")]
        public virtual UserOnTable CurrentTable { get; set; }
    }
}
