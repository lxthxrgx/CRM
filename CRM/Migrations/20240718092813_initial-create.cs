using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "D1",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        NameGroup = table.Column<string>(type: "TEXT", nullable: false),
            //        Fullname = table.Column<string>(type: "TEXT", nullable: false),
            //        rnokpp = table.Column<string>(type: "TEXT", nullable: false),
            //        address = table.Column<string>(type: "TEXT", nullable: false),
            //        edryofop_Data = table.Column<string>(type: "TEXT", nullable: false),
            //        BanckAccount = table.Column<string>(type: "TEXT", nullable: false),
            //        Director = table.Column<string>(type: "TEXT", nullable: false),
            //        ResPerson = table.Column<string>(type: "TEXT", nullable: true),
            //        Phone = table.Column<string>(type: "TEXT", nullable: true),
            //        Email = table.Column<string>(type: "TEXT", nullable: true),
            //        Tov = table.Column<bool>(type: "INTEGER", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_D1", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "D2",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        NumberGroup = table.Column<int>(type: "INTEGER", nullable: false),
            //        NameGroup = table.Column<string>(type: "TEXT", nullable: false),
            //        PIBS = table.Column<string>(type: "TEXT", nullable: true),
            //        address = table.Column<string>(type: "TEXT", nullable: false),
            //        area = table.Column<double>(type: "REAL", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_D2", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "D3",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        NumberGroup = table.Column<int>(type: "INTEGER", nullable: false),
            //        NameGroup = table.Column<string>(type: "TEXT", nullable: false),
            //        address = table.Column<string>(type: "TEXT", nullable: false),
            //        DogovirOrendu = table.Column<string>(type: "TEXT", nullable: false),
            //        DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
            //        StrokDii = table.Column<DateTime>(type: "TEXT", nullable: false),
            //        Suma = table.Column<string>(type: "TEXT", nullable: false),
            //        AktDate = table.Column<DateTime>(type: "TEXT", nullable: false),
            //        Stan = table.Column<string>(type: "TEXT", nullable: true),
            //        Prumitka = table.Column<string>(type: "TEXT", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_D3", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "D4",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        NumberGroup = table.Column<int>(type: "INTEGER", nullable: false),
            //        NameGroup = table.Column<string>(type: "TEXT", nullable: false),
            //        address = table.Column<string>(type: "TEXT", nullable: false),
            //        DogovirSuborendu = table.Column<string>(type: "TEXT", nullable: false),
            //        DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
            //        StrokDii = table.Column<DateTime>(type: "TEXT", nullable: false),
            //        Suma = table.Column<string>(type: "TEXT", nullable: false),
            //        AktDate = table.Column<DateTime>(type: "TEXT", nullable: false),
            //        Stan = table.Column<string>(type: "TEXT", nullable: true),
            //        Prumitka = table.Column<string>(type: "TEXT", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_D4", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "D5",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        NumberGroup = table.Column<int>(type: "INTEGER", nullable: false),
            //        address = table.Column<string>(type: "TEXT", nullable: false),
            //        OhronnaComp = table.Column<string>(type: "TEXT", nullable: false),
            //        NumDog = table.Column<string>(type: "TEXT", nullable: false),
            //        StrokDii = table.Column<DateTime>(type: "TEXT", nullable: false),
            //        ResPerson = table.Column<string>(type: "TEXT", nullable: true),
            //        Phone = table.Column<string>(type: "TEXT", nullable: true),
            //        PathToFile = table.Column<string>(type: "TEXT", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_D5", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "D6",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        NameGroup = table.Column<string>(type: "TEXT", nullable: false),
            //        NameTov = table.Column<string>(type: "TEXT", nullable: false),
            //        UnifiedStateRegister = table.Column<string>(type: "TEXT", nullable: false),
            //        address = table.Column<string>(type: "TEXT", nullable: false),
            //        director = table.Column<string>(type: "TEXT", nullable: false),
            //        BanckAccount = table.Column<string>(type: "TEXT", nullable: false),
            //        ResPerson = table.Column<string>(type: "TEXT", nullable: true),
            //        Phone = table.Column<string>(type: "TEXT", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_D6", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "status",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        status = table.Column<string>(type: "TEXT", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_status", x => x.Id);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "D1");

            //migrationBuilder.DropTable(
            //    name: "D2");

            //migrationBuilder.DropTable(
            //    name: "D3");

            //migrationBuilder.DropTable(
            //    name: "D4");

            //migrationBuilder.DropTable(
            //    name: "D5");

            //migrationBuilder.DropTable(
            //    name: "D6");

            //migrationBuilder.DropTable(
            //    name: "status");
        }
    }
}
