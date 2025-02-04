using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllowDeleteFlagDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Change the default value of 'allow_delete_flag' to true
            migrationBuilder.AlterColumn<bool>(
                name: "allow_delete_flag",
                table: "CustomView",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            // Update existing rows to set 'allow_delete_flag' to true
            migrationBuilder.Sql("UPDATE CustomView SET allow_delete_flag = 1 WHERE allow_delete_flag = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the default value of 'allow_delete_flag' back to false
            migrationBuilder.AlterColumn<bool>(
                name: "allow_delete_flag",
                table: "CustomView",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            // Optionally revert the existing rows back to false (if needed)
            migrationBuilder.Sql("UPDATE CustomView SET allow_delete_flag = 0 WHERE allow_delete_flag = 1");
        }
    }
}
