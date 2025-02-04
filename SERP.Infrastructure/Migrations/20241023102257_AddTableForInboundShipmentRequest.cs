using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableForInboundShipmentRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Containers_inbound_shipment_id_container_no",
                table: "Containers");

            migrationBuilder.RenameColumn(
                name: "issuing_branch_plant_id",
                table: "InboundShipments",
                newName: "branch_plant_id");

            migrationBuilder.AlterColumn<int>(
                name: "inbound_shipment_id",
                table: "Containers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "inbound_shipment_request_id",
                table: "Containers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "inbound_shipment_request_id",
                table: "AsnHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InboundShipmentFiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inbound_shipment_id = table.Column<int>(type: "int", nullable: false),
                    file_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundShipmentFiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InboundShipmentRequests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inbound_shipment_request_no = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    inbound_shipment_id = table.Column<int>(type: "int", nullable: true),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    freight_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    shipment_arranged_supplier_flag = table.Column<bool>(type: "bit", nullable: false),
                    incoterm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    cargo_ready_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    port_of_loading_etd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    port_of_discharge_eta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cargo_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    country_of_loading_id = table.Column<int>(type: "int", nullable: true),
                    port_of_loading_id = table.Column<int>(type: "int", nullable: true),
                    country_of_discharge_id = table.Column<int>(type: "int", nullable: true),
                    port_of_discharge_id = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundShipmentRequests", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_inbound_shipment_id_inbound_shipment_request_id_container_no",
                table: "Containers",
                columns: new[] { "inbound_shipment_id", "inbound_shipment_request_id", "container_no" },
                unique: true,
                filter: "[inbound_shipment_id] IS NOT NULL AND [inbound_shipment_request_id] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboundShipmentFiles");

            migrationBuilder.DropTable(
                name: "InboundShipmentRequests");

            migrationBuilder.DropIndex(
                name: "IX_Containers_inbound_shipment_id_inbound_shipment_request_id_container_no",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "inbound_shipment_request_id",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "inbound_shipment_request_id",
                table: "AsnHeaders");

            migrationBuilder.RenameColumn(
                name: "branch_plant_id",
                table: "InboundShipments",
                newName: "issuing_branch_plant_id");

            migrationBuilder.AlterColumn<int>(
                name: "inbound_shipment_id",
                table: "Containers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Containers_inbound_shipment_id_container_no",
                table: "Containers",
                columns: new[] { "inbound_shipment_id", "container_no" },
                unique: true);
        }
    }
}
