using Microsoft.EntityFrameworkCore;
using RepChecker.DtoModels;
using System.Collections.Generic;

namespace RepChecker.Data
{
    public class ReputationDbContext : DbContext
    {
        // Define here database tables DbSet<Dto>
        public DbSet<ApplicationUserModelDto> ApplicationUsers { get; set; }
        public DbSet<ReputationModelDto> UserReputations { get; set; }
        public DbSet<StandingModelDto> Standings { get; set; }

        public ReputationDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = AppLocalData.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ReputationModelDto>().HasOne(p => p.ApplicationUser).WithMany(e => e.UserReputations);
            //modelBuilder.Entity<ApplicationUserModelDto>().HasData(GetAppUserModel());
            //modelBuilder.Entity<ReputationModelDto>().HasData(GetReputationModel());
            modelBuilder.Entity<ApplicationUserModelDto>()
                .HasMany(e => e.UserReputations)
                .WithOne(b => b.ApplicationUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReputationModelDto>()
                .HasOne(e => e.Standing)
                .WithOne(b => b.Reputation)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }

        private ApplicationUserModelDto GetAppUserModel() => new ApplicationUserModelDto
        {
            BattleTag = "Wat#1337",
            Id = 1
        };

        private List<ReputationModelDto> GetReputationModel()
        {
            var reps = new List<ReputationModelDto>()
            {
                new ReputationModelDto
                {
                    Character = "Player1",
                    FactionHref = "https://eu.api.blizzard.com/blablabla/3",
                    ReputationId = 1,
                    Realm = "EU-Ragnaros",
                    ReputationName = "The Bamboocha"
                },
                new ReputationModelDto
                {
                    Character = "Player2",
                    FactionHref = "https://eu.api.blizzard.com/blablabla/2",
                    ReputationId = 2,
                    Realm = "EU-Ragnaros",
                    ReputationName = "The Bamboocha"
                },

            };

            return reps;
        }
    }
}
