using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRMAgreement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServerBD0208 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER update_1D_after_update
                AFTER UPDATE OF NumberGroup ON D2
                FOR EACH ROW
                BEGIN
                UPDATE D1
                SET numvid = NEW.NumberGroup
                WHERE numvid = OLD.NumberGroup;
                END
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER update_3D_after_update
                AFTER UPDATE OF NumberGroup, NameGroup, address ON D2
                FOR EACH ROW
                BEGIN
                UPDATE D3
                SET NumberGroup2D = NEW.NumberGroup,
                NameGroup2D = NEW.NameGroup,
                address = NEW.address
                WHERE NumberGroup2D = OLD.NumberGroup;
                END
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER update_4D_after_update
                AFTER UPDATE OF NumberGroup, NameGroup, address ON D2
                FOR EACH ROW
                BEGIN
                UPDATE D4
                SET NumberGroup2D = NEW.NumberGroup,
                NameGroup2D = NEW.NameGroup,
                address = NEW.address
                WHERE NumberGroup2D = OLD.NumberGroup;
                END
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER update_D5_after_update
                AFTER UPDATE OF NumberGroup, address ON D2
                FOR EACH ROW
                BEGIN
                UPDATE D5
                SET NumberGroup2D = NEW.NumberGroup,
                address = NEW.address
                WHERE NumberGroup2D = OLD.NumberGroup;
                END
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER update_all_NameGroup_after_update
                AFTER UPDATE OF NameGroup ON D2
                FOR EACH ROW
                BEGIN
                UPDATE D2
                SET NameGroup = NEW.NameGroup
                WHERE NameGroup = OLD.NameGroup;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_1D_after_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_3D_after_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_4D_after_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_D5_after_update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_all_NameGroup_after_update;");
        }
    }
}
