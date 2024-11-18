using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class AddToRent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "equipment",
                table: "D2",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rent",
                table: "D2",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "equipment",
                table: "D2");

            migrationBuilder.DropColumn(
                name: "rent",
                table: "D2");
        }
    }
}
