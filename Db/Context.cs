using Announcer.Models;
using Microsoft.EntityFrameworkCore;

namespace Announcer.Db
{
    public class Context : DbContext
    {
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<PlatformAlias> PlatformAlias { get; set; }
        public DbSet<DataUpdate> DataUpdates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./Announcer.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GamePlatform>().HasKey(x => new { x.GameId, x.PlatformAbbreviation });

            modelBuilder.Entity<GamePlatform>()
                .HasOne(gp => gp.Game)
                .WithMany(p => p.GamePlatforms)
                .HasForeignKey(gp => gp.GameId);

            modelBuilder.Entity<GamePlatform>()
                .HasOne(gp => gp.Platform)
                .WithMany(p => p.GamePlatforms)
                .HasForeignKey(gp => gp.PlatformAbbreviation);
        }
    }
}
