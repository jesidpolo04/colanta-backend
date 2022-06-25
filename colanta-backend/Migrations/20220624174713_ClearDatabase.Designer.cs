﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using colanta_backend.App.Shared.Infraestructure;

namespace colanta_backend.Migrations
{
    [DbContext(typeof(ColantaContext))]
    [Migration("20220624213345_ClearDatabase")]
    partial class ClearDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("colanta_backend.App.Brands.Infraestructure.EFBrand", b =>
                {
                    b.Property<int?>("id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("business");

                    b.Property<string>("id_siesa")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("id_siesa");

                    b.Property<int?>("id_vtex")
                        .HasColumnType("int")
                        .HasColumnName("id_vtex");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("name");

                    b.Property<short?>("state")
                        .HasColumnType("smallint")
                        .HasColumnName("state");

                    b.HasKey("id");

                    b.ToTable("brands");
                });

            modelBuilder.Entity("colanta_backend.App.Categories.Infraestructure.EFCategory", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("family")
                        .HasColumnType("int");

                    b.Property<bool>("isActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("siesa_id")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("siesa_id");

                    b.Property<int?>("vtex_id")
                        .HasColumnType("int")
                        .HasColumnName("vtex_id");

                    b.HasKey("id");

                    b.HasIndex("family");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("colanta_backend.App.CustomerCredit.Infraestructure.EFCreditAccount", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("credit_limit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("current_credit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("document")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("is_active")
                        .HasColumnType("bit");

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.Property<decimal>("vtex_credit_limit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("vtex_current_credit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("vtex_id")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("credit_accounts");
                });

            modelBuilder.Entity("colanta_backend.App.GiftCards.Infraestructure.EFGiftCard", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("emision_date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("expire_date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("owner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("siesa_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("giftcards");
                });

            modelBuilder.Entity("colanta_backend.App.Inventory.Infraestructure.EFInventory", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<string>("sku_concat_siesa_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("sku_id")
                        .HasColumnType("int");

                    b.Property<int>("warehouse_id")
                        .HasColumnType("int");

                    b.Property<string>("warehouse_siesa_id")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("sku_id");

                    b.HasIndex("warehouse_id");

                    b.ToTable("inventories");
                });

            modelBuilder.Entity("colanta_backend.App.Inventory.Infraestructure.EFWarehouse", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("siesa_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vtex_id")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("warehouses");
                });

            modelBuilder.Entity("colanta_backend.App.Orders.Infraestructure.EFOrder", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("current_change_date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("last_change_date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("last_status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("order_json")
                        .HasColumnType("text");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vtex_id")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("colanta_backend.App.Orders.SiesaOrders.Infraestructure.EFSiesaOrder", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("co")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cond_pago")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("doc_tercero")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fecha")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fecha_entrega")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("negocio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("notas")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("referencia_vtex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("siesa_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("total_descuento")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("total_pedido")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("id");

                    b.ToTable("siesa_orders");
                });

            modelBuilder.Entity("colanta_backend.App.Orders.SiesaOrders.Infraestructure.EFSiesaOrderDetail", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<string>("det_co")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("impuesto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ind_obsequio")
                        .HasColumnType("int");

                    b.Property<string>("notas")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("nro_detalle")
                        .HasColumnType("int");

                    b.Property<int>("order_id")
                        .HasColumnType("int");

                    b.Property<decimal>("precio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("referencia_item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("referencia_vtex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("unidad_medida")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("variacion_item")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("order_id");

                    b.ToTable("siesa_order_details");
                });

            modelBuilder.Entity("colanta_backend.App.Orders.SiesaOrders.Infraestructure.EFSiesaOrderDiscount", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("desto_co")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("nro_detalle")
                        .HasColumnType("int");

                    b.Property<int>("orden_descuento")
                        .HasColumnType("int");

                    b.Property<int>("order_id")
                        .HasColumnType("int");

                    b.Property<string>("referencia_vtex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("tasa")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("id");

                    b.HasIndex("order_id");

                    b.ToTable("siesa_order_discounts");
                });

            modelBuilder.Entity("colanta_backend.App.Prices.Infraestructure.EFPrice", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("sku_concat_siesa_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("sku_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("sku_id")
                        .IsUnique();

                    b.ToTable("prices");
                });

            modelBuilder.Entity("colanta_backend.App.Products.Infraestructure.EFProduct", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("brand_id")
                        .HasColumnType("int");

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("business");

                    b.Property<int>("category_id")
                        .HasColumnType("int");

                    b.Property<string>("concat_siesa_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("concat_siesa_id");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<bool>("is_active")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("ref_id")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ref_id");

                    b.Property<string>("siesa_id")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("siesa_id");

                    b.Property<int?>("type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.Property<int?>("vtex_id")
                        .HasColumnType("int")
                        .HasColumnName("vtex_id");

                    b.HasKey("id");

                    b.HasIndex("brand_id");

                    b.HasIndex("category_id");

                    b.ToTable("products");
                });

            modelBuilder.Entity("colanta_backend.App.Products.Infraestructure.EFSku", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("concat_siesa_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("is_active")
                        .HasColumnType("bit");

                    b.Property<string>("measurement_unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("packaged_height")
                        .HasColumnType("float");

                    b.Property<double>("packaged_length")
                        .HasColumnType("float");

                    b.Property<double>("packaged_weight_kg")
                        .HasColumnType("float");

                    b.Property<double>("packaged_width")
                        .HasColumnType("float");

                    b.Property<int>("product_id")
                        .HasColumnType("int");

                    b.Property<string>("ref_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("siesa_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("unit_multiplier")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("vtex_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("product_id");

                    b.ToTable("skus");
                });

            modelBuilder.Entity("colanta_backend.App.Promotions.Infraestructure.EFPromotion", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("begin_date_utc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("brands_ids")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("categories_ids")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("concat_siesa_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("cumulative")
                        .HasColumnType("bit");

                    b.Property<string>("discount_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("end_date_utc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("gift_quantity_selectable")
                        .HasColumnType("int");

                    b.Property<string>("gifts_ids")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("is_active")
                        .HasColumnType("bit");

                    b.Property<string>("list_sku_1_buy_together_ids")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("list_sku_2_buy_together_ids")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("max_number_of_affected_items")
                        .HasColumnType("int");

                    b.Property<string>("max_number_of_affected_items_group_key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("minimum_quantity_buy_together")
                        .HasColumnType("int");

                    b.Property<bool>("multiple_use_per_client")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("nominal_discount_value")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("percentual_discount_value")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("percentual_discount_value_list_1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("percentual_discount_value_list_2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("percentual_shipping_discount_value")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("products_ids")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("quantity_to_affect_buy_together")
                        .HasColumnType("int");

                    b.Property<string>("siesa_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("skus_ids")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("total_value_celing")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("total_value_floor")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vtex_id")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("promotions");
                });

            modelBuilder.Entity("colanta_backend.App.Shared.Infraestructure.EFProcess", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("dateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("dateTime")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("json_details")
                        .HasColumnType("text")
                        .HasColumnName("json_details");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("content");

                    b.Property<int>("total_errors")
                        .HasColumnType("int")
                        .HasColumnName("total_errors");

                    b.Property<int>("total_loads")
                        .HasColumnType("int")
                        .HasColumnName("total_loads");

                    b.Property<int>("total_not_procecced")
                        .HasColumnType("int")
                        .HasColumnName("total_not_procecced");

                    b.Property<int>("total_obtained")
                        .HasColumnType("int")
                        .HasColumnName("total_obtained");

                    b.HasKey("id");

                    b.ToTable("process");
                });

            modelBuilder.Entity("colanta_backend.App.Users.Infraestructure.EFClientType", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("siesa_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("client_types");
                });

            modelBuilder.Entity("colanta_backend.App.Users.Infraestructure.EFUser", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("born_date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("business")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("city_code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("client_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("country_code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("department_code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("document")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("document_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("last_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("second_last_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telephone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("colanta_backend.App.Categories.Infraestructure.EFCategory", b =>
                {
                    b.HasOne("colanta_backend.App.Categories.Infraestructure.EFCategory", "father")
                        .WithMany("childs")
                        .HasForeignKey("family");

                    b.Navigation("father");
                });

            modelBuilder.Entity("colanta_backend.App.Inventory.Infraestructure.EFInventory", b =>
                {
                    b.HasOne("colanta_backend.App.Products.Infraestructure.EFSku", "sku")
                        .WithMany()
                        .HasForeignKey("sku_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("colanta_backend.App.Inventory.Infraestructure.EFWarehouse", "warehouse")
                        .WithMany()
                        .HasForeignKey("warehouse_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("sku");

                    b.Navigation("warehouse");
                });

            modelBuilder.Entity("colanta_backend.App.Orders.SiesaOrders.Infraestructure.EFSiesaOrderDetail", b =>
                {
                    b.HasOne("colanta_backend.App.Orders.SiesaOrders.Infraestructure.EFSiesaOrder", "order")
                        .WithMany("detalles")
                        .HasForeignKey("order_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("order");
                });

            modelBuilder.Entity("colanta_backend.App.Orders.SiesaOrders.Infraestructure.EFSiesaOrderDiscount", b =>
                {
                    b.HasOne("colanta_backend.App.Orders.SiesaOrders.Infraestructure.EFSiesaOrder", "order")
                        .WithMany("descuentos")
                        .HasForeignKey("order_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("order");
                });

            modelBuilder.Entity("colanta_backend.App.Prices.Infraestructure.EFPrice", b =>
                {
                    b.HasOne("colanta_backend.App.Products.Infraestructure.EFSku", "sku")
                        .WithOne()
                        .HasForeignKey("colanta_backend.App.Prices.Infraestructure.EFPrice", "sku_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("sku");
                });

            modelBuilder.Entity("colanta_backend.App.Products.Infraestructure.EFProduct", b =>
                {
                    b.HasOne("colanta_backend.App.Brands.Infraestructure.EFBrand", "brand")
                        .WithMany()
                        .HasForeignKey("brand_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("colanta_backend.App.Categories.Infraestructure.EFCategory", "category")
                        .WithMany()
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("brand");

                    b.Navigation("category");
                });

            modelBuilder.Entity("colanta_backend.App.Products.Infraestructure.EFSku", b =>
                {
                    b.HasOne("colanta_backend.App.Products.Infraestructure.EFProduct", "product")
                        .WithMany("skus")
                        .HasForeignKey("product_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("product");
                });

            modelBuilder.Entity("colanta_backend.App.Categories.Infraestructure.EFCategory", b =>
                {
                    b.Navigation("childs");
                });

            modelBuilder.Entity("colanta_backend.App.Orders.SiesaOrders.Infraestructure.EFSiesaOrder", b =>
                {
                    b.Navigation("descuentos");

                    b.Navigation("detalles");
                });

            modelBuilder.Entity("colanta_backend.App.Products.Infraestructure.EFProduct", b =>
                {
                    b.Navigation("skus");
                });
#pragma warning restore 612, 618
        }
    }
}
