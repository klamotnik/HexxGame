using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexx.Server.Db.Entity
{
    [Table("Game")]
    public class Game : BaseEntity<Game>
    {
        [Key]
        public int ID { get; set; }
        
        public int Player1ID { get; set; }
        
        public int Player2ID { get; set; }

        [ForeignKey("Player1ID")]
        public User Player1 { get; set; }

        [ForeignKey("Player2ID")]
        public User Player2 { get; set; }

        public bool Ongoing { get; set; }
        
        public long StartGameTime { get; set; }
        
        public int TimePerPlayer { get; set; }
        
        public int BoardSize { get; set; }

        [Required]
        public byte[] Board { get; set; }
        
        public virtual ICollection<GameMove> Moves { get; set; }
    }
}
