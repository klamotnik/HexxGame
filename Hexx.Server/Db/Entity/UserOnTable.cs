using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexx.Server.Db.Entity
{
    [Table("UsersOnTable")]
    public class UserOnTable : BaseEntity<UserOnTable>
    {
        [Key]
        public int UserID { get; set; }

        public int TableID { get; set; }
        
        [ForeignKey("UserID")]
        public User User { get; set; }
        
        [ForeignKey("TableID")]
        public Table Table { get; set; }
    }
}
