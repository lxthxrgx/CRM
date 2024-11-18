using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteD4BuhAddNewRowD4Bookk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "D4Buh");

            migrationBuilder.AddColumn<DateTime>(
                name: "payments_term",
                table: "D4Bookk",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payments_term",
                table: "D4Bookk");

            migrationBuilder.CreateTable(
                name: "D4Buh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AktDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DogovirSuborendu = table.Column<string>(type: "TEXT", nullable: false),
                    EndAktDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NameGroup = table.Column<string>(type: "TEXT", nullable: false),
                    NumberGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    Suma = table.Column<string>(type: "TEXT", nullable: false),
                    Suma2 = table.Column<string>(type: "TEXT", nullable: true),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    payments_term = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_D4Buh", x => x.Id);
                });
        }
    }
}
