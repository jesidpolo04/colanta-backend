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
                    id = table.Column<int>(type: "int", nullable: false),
                    id_vtex = table.Column<int>(type: "int", nullable: true),
                    id_siesa = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    state = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DeltaBrands",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeltaBrands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Process",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    total_loads = table.Column<int>(type: "int", nullable: false),
                    total_errors = table.Column<int>(type: "int", nullable: false),
                    total_not_procecced = table.Column<int>(type: "int", nullable: false),
                    json_details = table.Column<string>(type: "text", nullable: true),
                    dateTime = table.Column<DateTime>(type: "dateTime", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Process", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "DeltaBrands");

            migrationBuilder.DropTable(
                name: "Process");
        }
    }
}
