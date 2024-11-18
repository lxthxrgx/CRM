using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class updated5addonetomany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathToFile",
                table: "D5");

            migrationBuilder.CreateTable(
                name: "pathToFilesGuard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _5dId = table.Column<int>(type: "INTEGER", nullable: false),
                    PathTOServerFiles = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pathToFilesGuard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pathToFilesGuard_D5__5dId",
                        column: x => x._5dId,
                        principalTable: "D5",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pathToFilesGuard__5dId",
                table: "pathToFilesGuard",
                column: "_5dId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pathToFilesGuard");

            migrationBuilder.AddColumn<string>(
                name: "PathToFile",
                table: "D5",
                type: "TEXT",
                nullable: true);
        }
    }
}
