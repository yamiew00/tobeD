using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ExamKeeperClassLibrary.Models.Configs
{
    public partial class project_configContext : DbContext
    {
        private readonly string ConnectionString;

        private readonly ServerVersion ServerVersion;

        public project_configContext(string connectionString, ServerVersion serverVersion)
        {
            ConnectionString = connectionString;
            ServerVersion = serverVersion;
        }

        public project_configContext(DbContextOptions<project_configContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, ServerVersion);
        }

        public virtual DbSet<ExamkeeperConfig> ExamkeeperConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<ExamkeeperConfig>(entity =>
            {
                entity.HasKey(e => new { e.Version, e.Key })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("examkeeperConfig");

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Dev)
                    .HasMaxLength(255)
                    .HasColumnName("dev");

                entity.Property(e => e.Release)
                    .HasMaxLength(255)
                    .HasColumnName("release");

                entity.Property(e => e.Uat)
                    .HasMaxLength(255)
                    .HasColumnName("uat");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
