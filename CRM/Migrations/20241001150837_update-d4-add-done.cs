using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class updated4adddone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Less_than_year",
                table: "D4");

            migrationBuilder.DropColumn(
                name: "Prumitka",
                table: "D4");

            migrationBuilder.DropColumn(
                name: "Stan",
                table: "D4");

            migrationBuilder.DropColumn(
                name: "StrokDii",
                table: "D4");

            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "D4",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Done",
                table: "D4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Less_than_year",
                table: "D4",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Prumitka",
                table: "D4",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stan",
                table: "D4",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StrokDii",
                table: "D4",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
