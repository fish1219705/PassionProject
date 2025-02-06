using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class review : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DessertId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DessertName",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewTime",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "DessertIngredient",
                columns: table => new
                {
                    DessertsDessertId = table.Column<int>(type: "int", nullable: false),
                    IngredientsIngredientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DessertIngredient", x => new { x.DessertsDessertId, x.IngredientsIngredientId });
                    table.ForeignKey(
                        name: "FK_DessertIngredient_Desserts_DessertsDessertId",
                        column: x => x.DessertsDessertId,
                        principalTable: "Desserts",
                        principalColumn: "DessertId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DessertIngredient_Ingredients_IngredientsIngredientId",
                        column: x => x.IngredientsIngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DessertId",
                table: "Reviews",
                column: "DessertId");

            migrationBuilder.CreateIndex(
                name: "IX_DessertIngredient_IngredientsIngredientId",
                table: "DessertIngredient",
                column: "IngredientsIngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Desserts_DessertId",
                table: "Reviews",
                column: "DessertId",
                principalTable: "Desserts",
                principalColumn: "DessertId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Desserts_DessertId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "DessertIngredient");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_DessertId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "DessertId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "DessertName",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewTime",
                table: "Reviews");
        }
    }
}
