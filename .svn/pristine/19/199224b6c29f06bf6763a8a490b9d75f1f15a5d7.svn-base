using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CustomviewDeleteFlagRenameToAllowUpdateDeleteFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "allow_delete_flag",
                table: "CustomView",
                newName: "allow_update_delete_flag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "allow_update_delete_flag",
                table: "CustomView",
                newName: "allow_delete_flag");
        }
    }
}
