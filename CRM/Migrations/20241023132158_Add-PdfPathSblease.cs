using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class AddPdfPathSblease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PdfFilePath_Sublease",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _4DId = table.Column<int>(type: "INTEGER", nullable: false),
                    PathToPdfFile_Sublease = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfFilePath_Sublease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PdfFilePath_Sublease_D4__4DId",
                        column: x => x._4DId,
                        principalTable: "D4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PdfFilePath_Sublease__4DId",
                table: "PdfFilePath_Sublease",
                column: "_4DId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdfFilePath_Sublease");
        }
    }
}
