using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedExtendedCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cargo_description",
                table: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "shipment_arranged_supplier_flag",
                table: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "shipment_arranged_supplier_flag",
                table: "InboundShipmentRequests");

            migrationBuilder.AddColumn<decimal>(
                name: "extended_cost",
                table: "PoDetails",
                type: "decimal(9,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "branch_plant_id",
                table: "InboundShipmentRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "shipment_arranged_supplier_flag",
                table: "AsnHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "extended_cost",
                table: "AsnDetails",
                type: "decimal(9,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "extended_cost",
                table: "PoDetails");

            migrationBuilder.DropColumn(
                name: "branch_plant_id",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "shipment_arranged_supplier_flag",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "extended_cost",
                table: "AsnDetails");

            migrationBuilder.AddColumn<string>(
                name: "cargo_description",
                table: "InboundShipments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "shipment_arranged_supplier_flag",
                table: "InboundShipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "shipment_arranged_supplier_flag",
                table: "InboundShipmentRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
