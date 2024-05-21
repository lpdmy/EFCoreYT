using EntityFrameworkCore.Data.Configurations.Entities;
using EntityFrameworkCore.Domain;
using EntityFrameworkCore.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Data
{
    public class FootballLeagueDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = FootballLeague_EfCore; Encrypt = False")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information )
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FLUENT API
            modelBuilder.Entity<Team>()
                .HasMany(m => m.HomeMatches)
                .WithOne(m => m.HomeTeam)
                .HasForeignKey(m => m.HomeTeamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(m => m.AwayMatches)
                .WithOne(m => m.AwayTeam)
                .HasForeignKey(m => m.AwayTeamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.ApplyConfiguration(new LeagueSeedConfiguration());
            modelBuilder.ApplyConfiguration(new TeamSeedConfiguration());
            modelBuilder.ApplyConfiguration(new CoachSeedConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)        
        {
            var entries = ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Modified);
            foreach (var entry in entries)
            {
                var auditableObject = (BaseDomainObject) entry.Entity;
                auditableObject.ModifiedDate = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    auditableObject.CreatedDate = DateTime.Now;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Coach> Coaches { get; set; }
    }
}
