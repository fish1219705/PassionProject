using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class dessertIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DessertIngredient_Desserts_DessertsDessertId",
                table: "DessertIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_DessertIngredient_Ingredients_IngredientsIngredientId",
                table: "DessertIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DessertIngredient",
                table: "DessertIngredient");

            migrationBuilder.RenameColumn(
                name: "IngredientsIngredientId",
                table: "DessertIngredient",
                newName: "IngredientId");

            migrationBuilder.RenameColumn(
                name: "DessertsDessertId",
                table: "DessertIngredient",
                newName: "DessertId");

            migrationBuilder.RenameIndex(
                name: "IX_DessertIngredient_IngredientsIngredientId",
                table: "DessertIngredient",
                newName: "IX_DessertIngredient_IngredientId");

            migrationBuilder.AddColumn<int>(
                name: "DessertIngredientId",
                table: "DessertIngredient",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "SpecificTag",
                table: "DessertIngredient",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DessertIngredient",
                table: "DessertIngredient",
                column: "DessertIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_DessertIngredient_DessertId",
                table: "DessertIngredient",
                column: "DessertId");

            migrationBuilder.AddForeignKey(
                name: "FK_DessertIngredient_Desserts_DessertId",
                table: "DessertIngredient",
                column: "DessertId",
                principalTable: "Desserts",
                principalColumn: "DessertId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DessertIngredient_Ingredients_IngredientId",
                table: "DessertIngredient",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DessertIngredient_Desserts_DessertId",
                table: "DessertIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_DessertIngredient_Ingredients_IngredientId",
                table: "DessertIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DessertIngredient",
                table: "DessertIngredient");

            migrationBuilder.DropIndex(
                name: "IX_DessertIngredient_DessertId",
                table: "DessertIngredient");

            migrationBuilder.DropColumn(
                name: "DessertIngredientId",
                table: "DessertIngredient");

            migrationBuilder.DropColumn(
                name: "SpecificTag",
                table: "DessertIngredient");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "DessertIngredient",
                newName: "IngredientsIngredientId");

            migrationBuilder.RenameColumn(
                name: "DessertId",
                table: "DessertIngredient",
                newName: "DessertsDessertId");

            migrationBuilder.RenameIndex(
                name: "IX_DessertIngredient_IngredientId",
                table: "DessertIngredient",
                newName: "IX_DessertIngredient_IngredientsIngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DessertIngredient",
                table: "DessertIngredient",
                columns: new[] { "DessertsDessertId", "IngredientsIngredientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DessertIngredient_Desserts_DessertsDessertId",
                table: "DessertIngredient",
                column: "DessertsDessertId",
                principalTable: "Desserts",
                principalColumn: "DessertId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DessertIngredient_Ingredients_IngredientsIngredientId",
                table: "DessertIngredient",
                column: "IngredientsIngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
