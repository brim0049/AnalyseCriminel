using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyseApi.Migrations
{
    /// <inheritdoc />
    public partial class finallll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relationship_Persons_PersonId",
                table: "Relationship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relationship",
                table: "Relationship");

            migrationBuilder.RenameTable(
                name: "Relationship",
                newName: "Relations");

            migrationBuilder.RenameIndex(
                name: "IX_Relationship_PersonId",
                table: "Relations",
                newName: "IX_Relations_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relations",
                table: "Relations",
                column: "RelationshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relations_Persons_PersonId",
                table: "Relations",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relations_Persons_PersonId",
                table: "Relations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relations",
                table: "Relations");

            migrationBuilder.RenameTable(
                name: "Relations",
                newName: "Relationship");

            migrationBuilder.RenameIndex(
                name: "IX_Relations_PersonId",
                table: "Relationship",
                newName: "IX_Relationship_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relationship",
                table: "Relationship",
                column: "RelationshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relationship_Persons_PersonId",
                table: "Relationship",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
