using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatusForBusinessUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status_flag",
                table: "BusinessUnit",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "E");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status_flag",
                table: "BusinessUnit");
        }
    }
}
