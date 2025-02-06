using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class dessertIngredient2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecificTag",
                table: "DessertIngredient",
                newName: "ChangeIngredientOption");

            migrationBuilder.AddColumn<string>(
                name: "SpecificTag",
                table: "Desserts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecificTag",
                table: "Desserts");

            migrationBuilder.RenameColumn(
                name: "ChangeIngredientOption",
                table: "DessertIngredient",
                newName: "SpecificTag");
        }
    }
}
