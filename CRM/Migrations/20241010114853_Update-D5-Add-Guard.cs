using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateD5AddGuard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NameGroup",
                table: "D5",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameGroup",
                table: "D5");
        }
    }
}
