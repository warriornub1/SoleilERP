using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePoHeaderTableAddnewFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "attachment_flag",
                table: "PoHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "notes_to_warehouse_flag",
                table: "PoHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "total_line_discount",
                table: "PoHeaders",
                type: "decimal(13,4)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attachment_flag",
                table: "PoHeaders");

            migrationBuilder.DropColumn(
                name: "notes_to_warehouse_flag",
                table: "PoHeaders");

            migrationBuilder.DropColumn(
                name: "total_line_discount",
                table: "PoHeaders");
        }
    }
}
