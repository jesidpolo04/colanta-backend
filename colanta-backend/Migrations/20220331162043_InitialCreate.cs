using Microsoft.EntityFrameworkCore.Migrations;

namespace colanta_backend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeltaBrands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Process",
                table: "Process");

            migrationBuilder.RenameTable(
                name: "Process",
                newName: "process");

            migrationBuilder.AddPrimaryKey(
                name: "PK_process",
                table: "process",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_process",
                table: "process");

            migrationBuilder.RenameTable(
                name: "process",
                newName: "Process");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Process",
                table: "Process",
                column: "id");

            migrationBuilder.CreateTable(
                name: "DeltaBrands",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    siesa_id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeltaBrands", x => x.id);
                });
        }
    }
}
