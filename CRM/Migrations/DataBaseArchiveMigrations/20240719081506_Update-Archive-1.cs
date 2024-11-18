using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations.DataBaseArchiveMigrations
{
    /// <inheritdoc />
    public partial class UpdateArchive1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Archive_3D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    NameGroup = table.Column<string>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    DogovirOrendu = table.Column<string>(type: "TEXT", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StrokDii = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Suma = table.Column<string>(type: "TEXT", nullable: false),
                    AktDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Stan = table.Column<string>(type: "TEXT", nullable: true),
                    Prumitka = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archive_3D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Archive_4D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    NameGroup = table.Column<string>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    DogovirSuborendu = table.Column<string>(type: "TEXT", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StrokDii = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Suma = table.Column<string>(type: "TEXT", nullable: false),
                    AktDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Stan = table.Column<string>(type: "TEXT", nullable: true),
                    Prumitka = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archive_4D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Archive_5D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    OhronnaComp = table.Column<string>(type: "TEXT", nullable: false),
                    NumDog = table.Column<string>(type: "TEXT", nullable: false),
                    StrokDii = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ResPerson = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    PathToFile = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archive_5D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Archive_6D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameGroup = table.Column<string>(type: "TEXT", nullable: false),
                    NameTov = table.Column<string>(type: "TEXT", nullable: false),
                    UnifiedStateRegister = table.Column<string>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    director = table.Column<string>(type: "TEXT", nullable: false),
                    BanckAccount = table.Column<string>(type: "TEXT", nullable: false),
                    ResPerson = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archive_6D", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Archive_3D");

            migrationBuilder.DropTable(
                name: "Archive_4D");

            migrationBuilder.DropTable(
                name: "Archive_5D");

            migrationBuilder.DropTable(
                name: "Archive_6D");
        }
    }
}
