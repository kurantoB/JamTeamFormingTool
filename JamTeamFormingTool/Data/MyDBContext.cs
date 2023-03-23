using JamTeamFormingTool.Models;
using Microsoft.EntityFrameworkCore;

namespace JamTeamFormingTool.Data
{
    public class MyDBContext : DbContext
    {
        public DbSet<CoverageSet> CoverageSets { get; set; } = null!;
        public DbSet<CoverageSetRoleCategory> CoverageSetRoleCategories { get; set; } = null!;
        public DbSet<JamTeamFormingSession> JamTeamFormingSessions { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<RoleTemplate> RoleTemplates { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=JamTeamFormingTool");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Role>()
                .HasMany(r => r.RoleCategories)
                .WithMany(c => c.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleCategoryRole",
                    j => j.HasOne<CoverageSetRoleCategory>().WithMany().OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<Role>().WithMany().OnDelete(DeleteBehavior.Cascade));

            modelBuilder
                .Entity<Role>()
                .HasMany(r => r.Participants)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "ParticipantRole",
                    j => j.HasOne<Participant>().WithMany().OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<Role>().WithMany().OnDelete(DeleteBehavior.Cascade));

            modelBuilder
                .Entity<Role>()
                .HasMany(r => r.Teams)
                .WithMany(t => t.OpenRoles)
                .UsingEntity<Dictionary<string, object>>(
                    "TeamRole",
                    j => j.HasOne<Team>().WithMany().OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<Role>().WithMany().OnDelete(DeleteBehavior.Cascade));
        }
    }
}
