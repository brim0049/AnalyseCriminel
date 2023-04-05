using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyseApi.Migrations
{
    /// <inheritdoc />
    public partial class rycv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NIN",
                table: "Persons",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Relation",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "Calls",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NIN",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Relation",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "Calls");
        }
    }
}
