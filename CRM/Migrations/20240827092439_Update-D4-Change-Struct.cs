using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateD4ChangeStruct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndAktDate",
                table: "D4",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndAktDate",
                table: "D4");
        }
    }
}
