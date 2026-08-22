using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcInformatiqueApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelJour3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipements_TypesEquipements_TypeEquipementIdType",
                table: "Equipements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypesEquipements",
                table: "TypesEquipements");

            migrationBuilder.RenameTable(
                name: "TypesEquipements",
                newName: "TypeEquipements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeEquipements",
                table: "TypeEquipements",
                column: "IdType");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipements_TypeEquipements_TypeEquipementIdType",
                table: "Equipements",
                column: "TypeEquipementIdType",
                principalTable: "TypeEquipements",
                principalColumn: "IdType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipements_TypeEquipements_TypeEquipementIdType",
                table: "Equipements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeEquipements",
                table: "TypeEquipements");

            migrationBuilder.RenameTable(
                name: "TypeEquipements",
                newName: "TypesEquipements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypesEquipements",
                table: "TypesEquipements",
                column: "IdType");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipements_TypesEquipements_TypeEquipementIdType",
                table: "Equipements",
                column: "TypeEquipementIdType",
                principalTable: "TypesEquipements",
                principalColumn: "IdType");
        }
    }
}
