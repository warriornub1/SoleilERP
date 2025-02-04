using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipASN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Containers_inbound_shipment_id_inbound_shipment_request_id_container_no",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "inbound_shipment_id",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "inbound_shipment_id",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "inbound_shipment_request_id",
                table: "Containers");

            migrationBuilder.AddColumn<int>(
                name: "forwarder_invoice_currency_id",
                table: "InboundShipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "shipping_invoice_currency_id",
                table: "InboundShipments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Containers_inbound_shipment_blawb_id_container_no",
                table: "Containers",
                columns: new[] { "inbound_shipment_blawb_id", "container_no" },
                unique: true,
                filter: "[inbound_shipment_blawb_id] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Containers_inbound_shipment_blawb_id_container_no",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "forwarder_invoice_currency_id",
                table: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "shipping_invoice_currency_id",
                table: "InboundShipments");

            migrationBuilder.AddColumn<int>(
                name: "inbound_shipment_id",
                table: "InboundShipmentRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "inbound_shipment_id",
                table: "Containers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "inbound_shipment_request_id",
                table: "Containers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Containers_inbound_shipment_id_inbound_shipment_request_id_container_no",
                table: "Containers",
                columns: new[] { "inbound_shipment_id", "inbound_shipment_request_id", "container_no" },
                unique: true,
                filter: "[inbound_shipment_id] IS NOT NULL AND [inbound_shipment_request_id] IS NOT NULL");
        }
    }
}
