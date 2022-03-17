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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
