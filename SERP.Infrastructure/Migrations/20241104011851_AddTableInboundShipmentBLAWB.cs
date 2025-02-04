using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableInboundShipmentBLAWB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bl_awb_cargo_description",
                table: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "bl_awb_no",
                table: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "bl_awb_total_gross_weight",
                table: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "bl_awb_total_packages",
                table: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "bl_awb_volume",
                table: "InboundShipments");

            migrationBuilder.AddColumn<int>(
                name: "inbound_shipment_blawb_id",
                table: "Containers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InboundShipmentBLAWBs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inbound_shipment_id = table.Column<int>(type: "int", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    bl_awb_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    bl_awb_total_packages = table.Column<int>(type: "int", nullable: true),
                    bl_awb_total_gross_weight = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    bl_awb_volume = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    bl_awb_cargo_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundShipmentBLAWBs", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboundShipmentBLAWBs");

            migrationBuilder.DropColumn(
                name: "inbound_shipment_blawb_id",
                table: "Containers");

            migrationBuilder.AddColumn<string>(
                name: "bl_awb_cargo_description",
                table: "InboundShipments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bl_awb_no",
                table: "InboundShipments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "bl_awb_total_gross_weight",
                table: "InboundShipments",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "bl_awb_total_packages",
                table: "InboundShipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "bl_awb_volume",
                table: "InboundShipments",
                type: "decimal(8,2)",
                nullable: true);
        }
    }
}
