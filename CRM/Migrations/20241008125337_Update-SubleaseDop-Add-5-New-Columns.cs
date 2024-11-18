using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubleaseDopAdd5NewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Certificate_Number",
                table: "SubleaseDop",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certificate_Series",
                table: "SubleaseDop",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Index_Number",
                table: "SubleaseDop",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Issued",
                table: "SubleaseDop",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Registration_Date",
                table: "SubleaseDop",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Certificate_Number",
                table: "SubleaseDop");

            migrationBuilder.DropColumn(
                name: "Certificate_Series",
                table: "SubleaseDop");

            migrationBuilder.DropColumn(
                name: "Index_Number",
                table: "SubleaseDop");

            migrationBuilder.DropColumn(
                name: "Issued",
                table: "SubleaseDop");

            migrationBuilder.DropColumn(
                name: "Registration_Date",
                table: "SubleaseDop");
        }
    }
}
