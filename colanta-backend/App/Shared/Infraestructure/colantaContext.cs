using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using colanta_backend.App.Brands.Infraestructure;

#nullable disable

namespace colanta_backend.App.Shared.Infraestructure
{
    public partial class colantaContext : DbContext
    {
        public colantaContext()
        {
        }

        public colantaContext(DbContextOptions<colantaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EFBrand> Brands { get; set; }
        public virtual DbSet<EFProcess> Process { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost; Database=colanta; User=sa; Password=Jesing0408");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EFBrand>(entity =>
            {
                entity.ToTable("brands");

                entity.Property(e => e.id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.id_siesa)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_siesa");

                entity.Property(e => e.id_vtex).HasColumnName("id_vtex");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.state).HasColumnName("state");
            });

            modelBuilder.Entity<EFProcess>(entity => {
                entity.ToTable("process");

                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.name).IsRequired().HasColumnName("content");
                entity.Property(e => e.total_loads).HasColumnName("total_loads");
                entity.Property(e => e.total_errors).HasColumnName("total_errors");
                entity.Property(e => e.total_not_procecced).HasColumnName("total_not_procecced");
                entity.Property(e => e.json_details).HasColumnType("text").HasColumnName("json_details");
                entity.Property(e => e.dateTime).HasColumnType("dateTime").HasDefaultValueSql("getdate()");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
