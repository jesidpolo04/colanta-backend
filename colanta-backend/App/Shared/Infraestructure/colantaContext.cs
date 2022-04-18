using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;


using colanta_backend.App.Brands.Infraestructure;
using colanta_backend.App.Categories.Infraestructure;

#nullable disable

namespace colanta_backend.App.Shared.Infraestructure
{
    public partial class ColantaContext : DbContext
    {
        IConfiguration Configuration;
        public ColantaContext(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public DbSet<EFBrand> Brands { get; set; }
        public DbSet<EFCategory> Categories { get; set; }
        public DbSet<EFProcess> Process { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=" + Configuration["DbHost"] + "; Database=" + Configuration["DbName"] + "; User=" + Configuration["DbUser"] + "; Password=" + Configuration["DbPassword"]);
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

                entity.Property(e => e.business).HasColumnName("business");

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

            modelBuilder.Entity<EFCategory>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.name).IsRequired().HasColumnName("name");
                entity.Property(e => e.siesa_id).HasColumnName("siesa_id");
                entity.Property(e => e.vtex_id).HasColumnName("vtex_id");
                entity.Property(e => e.isActive).IsRequired().HasColumnName("is_active");

                entity.HasMany(e => e.childs).WithOne(e => e.father).HasForeignKey("family");
                entity.HasOne(e => e.father).WithMany(e => e.childs);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
