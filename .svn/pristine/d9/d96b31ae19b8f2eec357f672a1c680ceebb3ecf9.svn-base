using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCountryAndSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "intermediary_supplier_id",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "country_numeric_code",
                table: "Country");

            migrationBuilder.AddColumn<int>(
                name: "group_supplier_id",
                table: "Supplier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "registered_site_id",
                table: "Supplier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "continent",
                table: "Country",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "country_idd",
                table: "Country",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "country_long_name",
                table: "Country",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "group_supplier_id",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "registered_site_id",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "continent",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "country_idd",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "country_long_name",
                table: "Country");

            migrationBuilder.AddColumn<int>(
                name: "intermediary_supplier_id",
                table: "Supplier",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "country_numeric_code",
                table: "Country",
                type: "int",
                maxLength: 3,
                nullable: false,
                defaultValue: 0);
        }
    }
}
