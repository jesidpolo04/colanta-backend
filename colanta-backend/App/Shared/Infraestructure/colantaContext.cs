using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;


using colanta_backend.App.Brands.Infraestructure;
using colanta_backend.App.Categories.Infraestructure;
using colanta_backend.App.Products.Infraestructure;
using colanta_backend.App.Users.Infraestructure;
using colanta_backend.App.Prices.Infraestructure;
using colanta_backend.App.Inventory.Infraestructure;
using colanta_backend.App.Promotions.Infraestructure;
using colanta_backend.App.CustomerCredit.Infraestructure;
using colanta_backend.App.GiftCards.Infraestructure;

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

        public virtual DbSet<EFUser> Users { get; set; }
        public virtual DbSet<EFBrand> Brands { get; set; }
        public virtual DbSet<EFCategory> Categories { get; set; }
        public virtual DbSet<EFProduct> Products { get; set; }
        public virtual DbSet<EFWarehouse> Warehouses { get; set; }
        public virtual DbSet<EFInventory> Inventories { get; set; }
        public virtual DbSet<EFPrice> Prices { get; set; }
        public virtual DbSet<EFSku> Skus { get; set; }
        public virtual DbSet<EFPromotion> Promotions { get; set; }
        public virtual DbSet<EFCreditAccount> CreditAccounts { get; set; }
        public virtual DbSet<EFGiftCard> GiftCards { get; set; }
        public virtual DbSet<EFProcess> Process { get; set; }

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
            modelBuilder.Entity<EFUser>(entity => {
                entity.ToTable("users");

                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.name).IsRequired().HasColumnName("name");
                entity.Property(e => e.email).IsRequired();
                entity.Property(e => e.telephone);
                entity.Property(e => e.document).IsRequired();
                entity.Property(e => e.document_type).IsRequired();
                entity.Property(e => e.client_type).IsRequired();
            });

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
                entity.Property(e => e.total_obtained).HasColumnName("total_obtained");
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

            modelBuilder.Entity<EFProduct>(entity =>
            {
                entity.ToTable("products");

                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.type).HasColumnName("type");
                entity.Property(e => e.name).IsRequired().HasColumnName("name");
                entity.Property(e => e.siesa_id).HasColumnName("siesa_id");
                entity.Property(e => e.concat_siesa_id).HasColumnName("concat_siesa_id").IsRequired();
                entity.Property(e => e.vtex_id).HasColumnName("vtex_id");
                entity.Property(e => e.is_active).HasColumnName("is_active");
                entity.Property(e => e.description).HasColumnName("description");
                entity.Property(e => e.ref_id).HasColumnName("ref_id");
                entity.Property(e => e.business).HasColumnName("business");

                entity.HasOne(e => e.brand).WithMany().HasForeignKey(e => e.brand_id);
                entity.HasOne(e => e.category).WithMany().HasForeignKey(e => e.category_id);
                entity.HasMany(e => e.skus).WithOne(e => e.product);
            });

            modelBuilder.Entity<EFPrice>(entity =>
            {
                entity.ToTable("prices");

                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.sku_concat_siesa_id).IsRequired();
                entity.Property(e => e.price);
                entity.Property(e => e.business);
                
                entity.HasOne(e => e.sku).WithOne().HasForeignKey<EFPrice>(e => e.sku_id);
            });

            modelBuilder.Entity<EFWarehouse>(entity =>
            {
                entity.ToTable("warehouses");

                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.name).IsRequired();
                entity.Property(e => e.siesa_id);
                entity.Property(e => e.vtex_id);
                entity.Property(e => e.business);
            });

            modelBuilder.Entity<EFInventory>(entity =>
            {
                entity.ToTable("inventories");

                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.sku_concat_siesa_id).IsRequired();
                entity.Property(e => e.warehouse_id).IsRequired();
                entity.Property(e => e.quantity);
                entity.Property(e => e.business);

                entity.HasOne(e => e.sku).WithMany().HasForeignKey(e => e.sku_id);
                entity.HasOne(e => e.warehouse).WithMany().HasForeignKey(e => e.warehouse_id);
            });


            modelBuilder.Entity<EFSku>(entity =>
            {
                entity.ToTable("skus");

                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.name).IsRequired();
                entity.Property(e => e.description).IsRequired();
                entity.Property(e => e.siesa_id);
                entity.Property(e => e.concat_siesa_id).IsRequired();
                entity.Property(e => e.vtex_id);
                entity.Property(e => e.measurement_unit);
                entity.Property(e => e.is_active);
                entity.Property(e => e.packaged_height);
                entity.Property(e => e.packaged_length);
                entity.Property(e => e.packaged_width);
                entity.Property(e => e.packaged_weight_kg);
                entity.Property(e => e.ref_id);
                entity.Property(e => e.unit_multiplier);

                entity.HasOne(e => e.product).WithMany(e => e.skus).HasForeignKey(e => e.product_id);
            });

            modelBuilder.Entity<EFPromotion>(entity =>
            {
                entity.ToTable("promotions");
                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.vtex_id);
                entity.Property(e => e.siesa_id);
                entity.Property(e => e.concat_siesa_id);
                entity.Property(e => e.business);
                entity.Property(e => e.type);
                entity.Property(e => e.discount_type);
                entity.Property(e => e.name).IsRequired();
                entity.Property(e => e.begin_date_utc);
                entity.Property(e => e.end_date_utc);
                entity.Property(e => e.nominal_discount_value);
                entity.Property(e => e.percentual_discount_value);
                entity.Property(e => e.percentual_shipping_discount_value);
                entity.Property(e => e.max_number_of_affected_items);
                entity.Property(e => e.max_number_of_affected_items_group_key);
                entity.Property(e => e.minimum_quantity_buy_together);
                entity.Property(e => e.quantity_to_affect_buy_together);
                entity.Property(e => e.products_ids);
                entity.Property(e => e.skus_ids);
                entity.Property(e => e.brands_ids);
                entity.Property(e => e.categories_ids);
                entity.Property(e => e.gifts_ids);
                entity.Property(e => e.list_sku_1_buy_together_ids);
                entity.Property(e => e.list_sku_2_buy_together_ids);
                entity.Property(e => e.total_value_floor);
                entity.Property(e => e.total_value_celing);

                entity.Property(e => e.is_active);
            });

            modelBuilder.Entity<EFCreditAccount>(entity =>
            {
                entity.ToTable("credit_accounts");
                
                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.vtex_id);
                entity.Property(e => e.business);
                entity.Property(e => e.credit_limit);
                entity.Property(e => e.is_active);

                entity.HasOne(e => e.user).WithMany().HasForeignKey(e => e.user_id);
            });

            modelBuilder.Entity<EFGiftCard>(entity =>
            {
                entity.ToTable("giftcards");

                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.siesa_id);
                entity.Property(e => e.name);
                entity.Property(e => e.balance);
                entity.Property(e => e.owner);
                entity.Property(e => e.business);
                entity.Property(e => e.code);
                entity.Property(e => e.token);
                entity.Property(e => e.expire_date);
                entity.Property(e => e.emision_date);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
