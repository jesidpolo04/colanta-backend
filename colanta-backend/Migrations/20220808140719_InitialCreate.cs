using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace colanta_backend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_vtex = table.Column<int>(type: "int", nullable: true),
                    id_siesa = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    state = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vtex_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    family = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_categories_categories_family",
                        column: x => x.family,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "client_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "credit_accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vtex_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    credit_limit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vtex_credit_limit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vtex_current_credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    current_credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "giftcards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    emision_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    expire_date = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_giftcards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "giftcards_transactions_authorizations",
                columns: table => new
                {
                    oid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_giftcards_transactions_authorizations", x => x.oid);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    throw_at = table.Column<DateTime>(type: "dateTime", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vtex_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    order_json = table.Column<string>(type: "text", nullable: true),
                    last_change_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    current_change_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vtex_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    json = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_methods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_promissory = table.Column<bool>(type: "bit", nullable: false),
                    vtex_id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "process",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    total_loads = table.Column<int>(type: "int", nullable: false),
                    total_errors = table.Column<int>(type: "int", nullable: false),
                    total_not_procecced = table.Column<int>(type: "int", nullable: false),
                    total_obtained = table.Column<int>(type: "int", nullable: false),
                    json_details = table.Column<string>(type: "text", nullable: true),
                    dateTime = table.Column<DateTime>(type: "dateTime", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vtex_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    concat_siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    discount_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    begin_date_utc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    end_date_utc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    maximum_unit_price_discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    nominal_discount_value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    percentual_discount_value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    percentual_shipping_discount_value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    max_number_of_affected_items_group_key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    max_number_of_affected_items = table.Column<int>(type: "int", nullable: false),
                    minimum_quantity_buy_together = table.Column<int>(type: "int", nullable: false),
                    quantity_to_affect_buy_together = table.Column<int>(type: "int", nullable: false),
                    products_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    skus_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brands_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    categories_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cluster_expressions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gift_quantity_selectable = table.Column<int>(type: "int", nullable: false),
                    gifts_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    percentual_discount_value_list_1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    percentual_discount_value_list_2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    list_sku_1_buy_together_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    list_sku_2_buy_together_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_value_floor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_value_celing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    cumulative = table.Column<bool>(type: "bit", nullable: false),
                    multiple_use_per_client = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "siesa_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    finalizado = table.Column<bool>(type: "bit", nullable: false),
                    co = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    doc_tercero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_entrega = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    referencia_vtex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado_vtex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_metodo_pago_vtex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metodo_pago_vtex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cond_pago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    departamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    negocio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_pedido = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    recoge_en_tienda = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_siesa_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "siesa_orders_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vtex_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    order_json = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_siesa_orders_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    second_last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    document_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    born_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    client_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "warehouses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vtex_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<int>(type: "int", nullable: true),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    concat_siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vtex_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ref_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    brand_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "giftcards_transactions",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    card_id = table.Column<int>(type: "int", nullable: false),
                    json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_authorization_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_giftcards_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_giftcards_transactions_giftcards_card_id",
                        column: x => x.card_id,
                        principalTable: "giftcards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_giftcards_transactions_giftcards_transactions_authorizations_transaction_authorization_id",
                        column: x => x.transaction_authorization_id,
                        principalTable: "giftcards_transactions_authorizations",
                        principalColumn: "oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "siesa_order_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    det_co = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nro_detalle = table.Column<int>(type: "int", nullable: false),
                    referencia_item = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    variacion_item = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ind_obsequio = table.Column<int>(type: "int", nullable: false),
                    unidad_medida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    impuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    referencia_vtex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    order_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_siesa_order_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_siesa_order_details_siesa_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "siesa_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "siesa_order_discounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    desto_co = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    referencia_vtex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nro_detalle = table.Column<int>(type: "int", nullable: false),
                    orden_descuento = table.Column<int>(type: "int", nullable: false),
                    tasa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    order_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_siesa_order_discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_siesa_order_discounts_siesa_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "siesa_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "skus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    vtex_id = table.Column<int>(type: "int", nullable: true),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    concat_siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ref_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    packaged_height = table.Column<double>(type: "float", nullable: false),
                    packaged_length = table.Column<double>(type: "float", nullable: false),
                    packaged_width = table.Column<double>(type: "float", nullable: false),
                    packaged_weight_kg = table.Column<double>(type: "float", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    measurement_unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit_multiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skus", x => x.id);
                    table.ForeignKey(
                        name: "FK_skus_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "giftcards_transactions_cancellations",
                columns: table => new
                {
                    oid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    transaction_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_giftcards_transactions_cancellations", x => x.oid);
                    table.ForeignKey(
                        name: "FK_giftcards_transactions_cancellations_giftcards_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "giftcards_transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "giftcards_transactions_settlements",
                columns: table => new
                {
                    oid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    transaction_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_giftcards_transactions_settlements", x => x.oid);
                    table.ForeignKey(
                        name: "FK_giftcards_transactions_settlements_giftcards_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "giftcards_transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inventories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    sku_id = table.Column<int>(type: "int", nullable: false),
                    sku_concat_siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    warehouse_id = table.Column<int>(type: "int", nullable: false),
                    warehouse_siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventories", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventories_skus_sku_id",
                        column: x => x.sku_id,
                        principalTable: "skus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventories_warehouses_warehouse_id",
                        column: x => x.warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sku_concat_siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sku_id = table.Column<int>(type: "int", nullable: false),
                    business = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prices", x => x.id);
                    table.ForeignKey(
                        name: "FK_prices_skus_sku_id",
                        column: x => x.sku_id,
                        principalTable: "skus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_family",
                table: "categories",
                column: "family");

            migrationBuilder.CreateIndex(
                name: "IX_giftcards_transactions_card_id",
                table: "giftcards_transactions",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_giftcards_transactions_transaction_authorization_id",
                table: "giftcards_transactions",
                column: "transaction_authorization_id",
                unique: true,
                filter: "[transaction_authorization_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_giftcards_transactions_cancellations_transaction_id",
                table: "giftcards_transactions_cancellations",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_giftcards_transactions_settlements_transaction_id",
                table: "giftcards_transactions_settlements",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventories_sku_id",
                table: "inventories",
                column: "sku_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventories_warehouse_id",
                table: "inventories",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_prices_sku_id",
                table: "prices",
                column: "sku_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_brand_id",
                table: "products",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_siesa_order_details_order_id",
                table: "siesa_order_details",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_siesa_order_discounts_order_id",
                table: "siesa_order_discounts",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_skus_product_id",
                table: "skus",
                column: "product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "client_types");

            migrationBuilder.DropTable(
                name: "credit_accounts");

            migrationBuilder.DropTable(
                name: "giftcards_transactions_cancellations");

            migrationBuilder.DropTable(
                name: "giftcards_transactions_settlements");

            migrationBuilder.DropTable(
                name: "inventories");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "order_status");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "orders_history");

            migrationBuilder.DropTable(
                name: "payment_methods");

            migrationBuilder.DropTable(
                name: "prices");

            migrationBuilder.DropTable(
                name: "process");

            migrationBuilder.DropTable(
                name: "promotions");

            migrationBuilder.DropTable(
                name: "siesa_order_details");

            migrationBuilder.DropTable(
                name: "siesa_order_discounts");

            migrationBuilder.DropTable(
                name: "siesa_orders_history");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "giftcards_transactions");

            migrationBuilder.DropTable(
                name: "warehouses");

            migrationBuilder.DropTable(
                name: "skus");

            migrationBuilder.DropTable(
                name: "siesa_orders");

            migrationBuilder.DropTable(
                name: "giftcards");

            migrationBuilder.DropTable(
                name: "giftcards_transactions_authorizations");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
