using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableForAsnAndInboundShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country_code",
                table: "Ports");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "Currency",
                newName: "currency_code");

            migrationBuilder.AddColumn<int>(
                name: "country_id",
                table: "Ports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "open_qty",
                table: "PoDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "currency_id",
                table: "CurrencyExchange",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<int>(
                name: "base_currency_id",
                table: "CurrencyExchange",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AddColumn<string>(
                name: "currency_description",
                table: "Currency",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AsnDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asn_header_id = table.Column<int>(type: "int", nullable: false),
                    line_no = table.Column<int>(type: "int", nullable: false),
                    po_detail_id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    unit_cost = table.Column<int>(type: "int", nullable: false),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: true),
                    notes_to_warehouse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsnDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AsnHeaders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asn_no = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    issuing_branch_plant_id = table.Column<int>(type: "int", nullable: false),
                    ship_to_branch_plant_id = table.Column<int>(type: "int", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    inbound_shipment_id = table.Column<int>(type: "int", nullable: true),
                    forecast_ex_wh_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    invoice_no = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    invoice_currency_id = table.Column<int>(type: "int", nullable: false),
                    invoice_amt = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    total_asn_amt = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    invoice_exchange_rate = table.Column<decimal>(type: "decimal(9,4)", nullable: true),
                    internal_remarks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    notes_to_cargo_team = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    total_packages = table.Column<int>(type: "int", nullable: true),
                    total_gross_weight = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    volume = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsnHeaders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InboundShipments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inbound_shipment_no = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    issuing_branch_plant_id = table.Column<int>(type: "int", nullable: false),
                    freight_method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shipment_arranged_supplier_flag = table.Column<bool>(type: "bit", nullable: false),
                    incoterm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cargo_ready_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    port_of_loading_etd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    port_of_discharge_eta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cargo_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country_of_loading_id = table.Column<int>(type: "int", nullable: false),
                    port_of_loading_id = table.Column<int>(type: "int", nullable: false),
                    country_of_discharge_id = table.Column<int>(type: "int", nullable: false),
                    port_of_discharge_id = table.Column<int>(type: "int", nullable: false),
                    bl_awb_total_packages = table.Column<int>(type: "int", nullable: true),
                    bl_awb_total_gross_weight = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    bl_awb_volume = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    bl_awb_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    vessel_flight_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    connecting_vessel_flight_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    notice_of_arrival_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    import_permit_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    internal_remarks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    forwarder_agent_id = table.Column<int>(type: "int", nullable: true),
                    forwarder_invoice_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    forwarder_invoice_amt = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    shipping_agent_id = table.Column<int>(type: "int", nullable: true),
                    shipping_invoice_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    shipping_invoice_amt = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    insurance_agent_id = table.Column<int>(type: "int", nullable: true),
                    bl_awb_cargo_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundShipments", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsnDetails");

            migrationBuilder.DropTable(
                name: "AsnHeaders");

            migrationBuilder.DropTable(
                name: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "country_id",
                table: "Ports");

            migrationBuilder.DropColumn(
                name: "open_qty",
                table: "PoDetails");

            migrationBuilder.DropColumn(
                name: "currency_description",
                table: "Currency");

            migrationBuilder.RenameColumn(
                name: "currency_code",
                table: "Currency",
                newName: "currency");

            migrationBuilder.AddColumn<string>(
                name: "country_code",
                table: "Ports",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "currency_id",
                table: "CurrencyExchange",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "base_currency_id",
                table: "CurrencyExchange",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
