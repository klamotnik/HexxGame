using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexx.Server.Db.Entity
{
    [Table("Tables")]
    public class Table : BaseEntity<Table>
    {
        [Key]
        public int Number { get; set; }

        public int BoardSize { get; set; }

        public int TimeForPlayer { get; set; }

        public int? Seat1 { get; set; }

        [ForeignKey("Seat1")]
        public User Player1 { get; set; }

        public int? Seat2 { get; set; }

        [ForeignKey("Seat2")]
        public User Player2 { get; set; }

        public int? GameID { get; set; }

        [ForeignKey("GameID")]
        public Game Game { get; set; }

        public virtual ICollection<UserOnTable> UsersOnTable { get; set; } = new List<UserOnTable>();
    }
}
