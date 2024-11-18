using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class AddSubleaseDopProd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubleaseDopId",
                table: "D2",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_D2_SubleaseDopId",
                table: "D2",
                column: "SubleaseDopId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_D2_SubleaseDop_SubleaseDopId",
                table: "D2",
                column: "SubleaseDopId",
                principalTable: "SubleaseDop",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_D2_SubleaseDop_SubleaseDopId",
                table: "D2");

            migrationBuilder.DropIndex(
                name: "IX_D2_SubleaseDopId",
                table: "D2");

            migrationBuilder.DropColumn(
                name: "SubleaseDopId",
                table: "D2");
        }
    }
}
