using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations.DataBaseArchiveMigrations
{
    /// <inheritdoc />
    public partial class UpdateArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prumitka",
                table: "Archive_4D");

            migrationBuilder.RenameColumn(
                name: "StrokDii",
                table: "Archive_4D",
                newName: "EndAktDate");

            migrationBuilder.RenameColumn(
                name: "Stan",
                table: "Archive_4D",
                newName: "Suma2");

            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "Archive_4D",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Done",
                table: "Archive_4D");

            migrationBuilder.RenameColumn(
                name: "Suma2",
                table: "Archive_4D",
                newName: "Stan");

            migrationBuilder.RenameColumn(
                name: "EndAktDate",
                table: "Archive_4D",
                newName: "StrokDii");

            migrationBuilder.AddColumn<string>(
                name: "Prumitka",
                table: "Archive_4D",
                type: "TEXT",
                nullable: true);
        }
    }
}
