using System.Collections.Generic;
using System.Data.SqlClient;
using Hexx.Server.Db.Entity;
using Microsoft.EntityFrameworkCore;

namespace Hexx.Server.Db
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<UserOnTable> UsersOnTable { get; set; }
        public virtual DbSet<UserData> UserData { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameMove> GameMoves { get; set; }

        public DatabaseContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DatabaseConnectionManager manager = DatabaseConnectionManager.GetInstance();
            optionsBuilder.UseSqlServer(manager.GetConnectionString());
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public void CreateUser(string user, string password)
        {
            Users.FromSql("EXEC CreateGameUser {0}, {1}", user, password);
        }
    }
}