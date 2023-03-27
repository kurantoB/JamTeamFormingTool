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

        public string DbPath { get; }

        public MyDBContext(IWebHostEnvironment webHostEnvironment) {
            DbPath = System.IO.Path.Join(webHostEnvironment.WebRootPath, "jamteamformingtool.db");
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        // optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=JamTeamFormingTool");
        //optionsBuilder.UseSqlServer(@"Server=tcp:jamteamformingtooldbserver.database.windows.net,1433;Initial Catalog=JamTeamFormingTool_db;Persist Security Info=False;User ID=jamteamformingtooladmin;Password=JamTeamFormingTool!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //}

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

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
