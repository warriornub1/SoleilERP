using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalAmtBaseAndTotalAmtForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "total_amt_base",
                table: "PoHeaders",
                type: "decimal(9,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amt_foreign",
                table: "PoHeaders",
                type: "decimal(11,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_amt_base",
                table: "PoHeaders");

            migrationBuilder.DropColumn(
                name: "total_amt_foreign",
                table: "PoHeaders");
        }
    }
}
