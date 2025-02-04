using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInboundShipmentForASN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "asn_header_id",
                table: "InboundShipmentRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "collection_address",
                table: "InboundShipmentRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "hs_code",
                table: "InboundShipmentRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "marking_bl",
                table: "InboundShipmentRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "marking_cargo",
                table: "InboundShipmentRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipment_type",
                table: "InboundShipmentRequests",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_gross_weight",
                table: "InboundShipmentRequests",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_packages",
                table: "InboundShipmentRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "volume",
                table: "InboundShipmentRequests",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bl_awb_provided",
                table: "AsnHeaders",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InboundShipmentASN",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inbound_shipment_id = table.Column<int>(type: "int", nullable: false),
                    asn_header_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundShipmentASN", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboundShipmentASN");

            migrationBuilder.DropColumn(
                name: "asn_header_id",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "collection_address",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "hs_code",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "marking_bl",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "marking_cargo",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "shipment_type",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "total_gross_weight",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "total_packages",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "volume",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "bl_awb_provided",
                table: "AsnHeaders");
        }
    }
}
