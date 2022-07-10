using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace colanta_backend.Migrations
{
    public partial class SiesaOrderHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "finalizado",
                table: "siesa_orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "siesa_orders_history");

            migrationBuilder.DropColumn(
                name: "finalizado",
                table: "siesa_orders");
        }
    }
}
