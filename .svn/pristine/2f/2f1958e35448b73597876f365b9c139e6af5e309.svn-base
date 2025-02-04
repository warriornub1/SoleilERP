using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnForAsnPortAndISG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "default_country_of_loading_id",
                table: "Supplier",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "default_port_of_loading_id",
                table: "Supplier",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "inbound_shipment_request_group_no",
                table: "InboundShipmentRequests",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bay_no",
                table: "Containers",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "variance_reason",
                table: "AsnHeaders",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "default_country_of_loading_id",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "default_port_of_loading_id",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "inbound_shipment_request_group_no",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "bay_no",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "variance_reason",
                table: "AsnHeaders");
        }
    }
}
