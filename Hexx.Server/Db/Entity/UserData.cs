using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexx.Server.Db.Entity
{
    [Table("UserData")]
    public class UserData : BaseEntity<UserData>
    {
        [Key]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        [Required]
        public int Rank { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
