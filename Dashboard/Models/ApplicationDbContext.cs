using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Dashboard.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySQL(Startup.Settings.ConnectionString);
        public virtual DbSet<Saved> Saveds { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Saved>(entity =>
            {
                entity.ToTable("saved");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnType("longtext")
                    .HasColumnName("data");

                entity.Property(e => e.Time).HasColumnName("time");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
