using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexx.Server.Db.Entity
{
    [Table("GameMoves")]
    public class GameMove : BaseEntity<GameMove>
    {
        [Key]
        public int GameID { get; set; }
        
        [ForeignKey("GameID")]
        public Game Game { get; set; }

        public int PlayerID { get; set; }

        [ForeignKey("PlayerID")]
        public User Player { get; set; }
        
        public int FieldFrom { get; set; }
        
        public int FieldTo { get; set; }
        
        public long Time { get; set; }
    }
}
