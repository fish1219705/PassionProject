using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class picture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPic",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PicExtension",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPic",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "PicExtension",
                table: "Reviews");
        }
    }
}
