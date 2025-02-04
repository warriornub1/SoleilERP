using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "company_id",
                table: "PoHeaders");

            migrationBuilder.DropColumn(
                name: "asn_header_id",
                table: "PackingListHeaders");

            migrationBuilder.DropColumn(
                name: "asn_detail_id",
                table: "PackingListDetails");

            migrationBuilder.DropColumn(
                name: "branch_plant_id",
                table: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "cargo_ready_date",
                table: "InboundShipments");

            migrationBuilder.DropColumn(
                name: "branch_plant_id",
                table: "InboundShipmentRequests");

            migrationBuilder.DropColumn(
                name: "supplier_id",
                table: "InboundShipmentBLAWBs");

            migrationBuilder.DropColumn(
                name: "invoice_amt",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "invoice_currency_id",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "invoice_exchange_rate",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "invoice_no",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "total_asn_amt",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "total_gross_weight",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "total_packages",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "variance_reason",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "volume",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "extended_cost",
                table: "AsnDetails");

            migrationBuilder.DropColumn(
                name: "po_detail_id",
                table: "AsnDetails");

            migrationBuilder.DropColumn(
                name: "unit_cost",
                table: "AsnDetails");

            migrationBuilder.RenameColumn(
                name: "inco_term",
                table: "Supplier",
                newName: "incoterm");

            migrationBuilder.RenameColumn(
                name: "issuing_branch_plant_id",
                table: "PoHeaders",
                newName: "branch_plant_id");

            migrationBuilder.RenameColumn(
                name: "inco_terms",
                table: "PoHeaders",
                newName: "incoterm");

            migrationBuilder.RenameColumn(
                name: "forwarder_id",
                table: "PoHeaders",
                newName: "forwarder_agent_id");

            migrationBuilder.RenameColumn(
                name: "collection_date",
                table: "PoHeaders",
                newName: "collection_date_time");

            migrationBuilder.RenameColumn(
                name: "ship_to_bp_id",
                table: "PoDetails",
                newName: "ship_to_branch_plant_id");

            migrationBuilder.RenameColumn(
                name: "collection_date",
                table: "PoDetails",
                newName: "collection_date_time");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amt_foreign",
                table: "PoHeaders",
                type: "decimal(13,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amt_base",
                table: "PoHeaders",
                type: "decimal(13,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "requested_date",
                table: "PoHeaders",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "quoted_ex_fac_date_latest",
                table: "PoHeaders",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "quoted_ex_fac_date_earliest",
                table: "PoHeaders",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "forecast_ex_wh_date",
                table: "PoHeaders",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ack_ex_fac_date",
                table: "PoHeaders",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_discount",
                table: "PoDetails",
                type: "decimal(11,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_cost",
                table: "PoDetails",
                type: "decimal(11,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,4)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "requested_date",
                table: "PoDetails",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "quoted_ex_fac_date_latest",
                table: "PoDetails",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "quoted_ex_fac_date_earliest",
                table: "PoDetails",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "forecast_ex_wh_date",
                table: "PoDetails",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "extended_cost",
                table: "PoDetails",
                type: "decimal(13,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ack_ex_fac_date",
                table: "PoDetails",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Lov",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "inspection_instruction",
                table: "Item",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "item_uom_conversion_id",
                table: "Item",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "label_required_flag",
                table: "Item",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "lot_control_flag",
                table: "Item",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "port_of_loading_etd",
                table: "InboundShipments",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "port_of_discharge_eta",
                table: "InboundShipments",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "notice_of_arrival_date",
                table: "InboundShipments",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "port_of_loading_etd",
                table: "InboundShipmentRequests",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "port_of_discharge_eta",
                table: "InboundShipmentRequests",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "cargo_ready_date",
                table: "InboundShipmentRequests",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cargo_description",
                table: "InboundShipmentRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<decimal>(
                name: "exchange_rate",
                table: "CurrencyExchange",
                type: "decimal(13,7)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)");

            migrationBuilder.AlterColumn<bool>(
                name: "intercompany_flag",
                table: "Company",
                type: "bit",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "dormant_flag",
                table: "Company",
                type: "bit",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "received_date",
                table: "AsnHeaders",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "forecast_ex_wh_date",
                table: "AsnHeaders",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "estimated_putaway_date",
                table: "AsnHeaders",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "Lov");

            migrationBuilder.DropColumn(
                name: "inspection_instruction",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "item_uom_conversion_id",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "label_required_flag",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "lot_control_flag",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "incoterm",
                table: "Supplier",
                newName: "inco_term");

            migrationBuilder.RenameColumn(
                name: "incoterm",
                table: "PoHeaders",
                newName: "inco_terms");

            migrationBuilder.RenameColumn(
                name: "forwarder_agent_id",
                table: "PoHeaders",
                newName: "forwarder_id");

            migrationBuilder.RenameColumn(
                name: "collection_date_time",
                table: "PoHeaders",
                newName: "collection_date");

            migrationBuilder.RenameColumn(
                name: "branch_plant_id",
                table: "PoHeaders",
                newName: "issuing_branch_plant_id");

            migrationBuilder.RenameColumn(
                name: "ship_to_branch_plant_id",
                table: "PoDetails",
                newName: "ship_to_bp_id");

            migrationBuilder.RenameColumn(
                name: "collection_date_time",
                table: "PoDetails",
                newName: "collection_date");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amt_foreign",
                table: "PoHeaders",
                type: "decimal(11,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amt_base",
                table: "PoHeaders",
                type: "decimal(9,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "requested_date",
                table: "PoHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "quoted_ex_fac_date_latest",
                table: "PoHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "quoted_ex_fac_date_earliest",
                table: "PoHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "forecast_ex_wh_date",
                table: "PoHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ack_ex_fac_date",
                table: "PoHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "PoHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_discount",
                table: "PoDetails",
                type: "decimal(8,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_cost",
                table: "PoDetails",
                type: "decimal(8,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,4)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "requested_date",
                table: "PoDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "quoted_ex_fac_date_latest",
                table: "PoDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "quoted_ex_fac_date_earliest",
                table: "PoDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "forecast_ex_wh_date",
                table: "PoDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "extended_cost",
                table: "PoDetails",
                type: "decimal(11,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ack_ex_fac_date",
                table: "PoDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "asn_header_id",
                table: "PackingListHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "asn_detail_id",
                table: "PackingListDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "port_of_loading_etd",
                table: "InboundShipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "port_of_discharge_eta",
                table: "InboundShipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "notice_of_arrival_date",
                table: "InboundShipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "branch_plant_id",
                table: "InboundShipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "cargo_ready_date",
                table: "InboundShipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "port_of_loading_etd",
                table: "InboundShipmentRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "port_of_discharge_eta",
                table: "InboundShipmentRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "cargo_ready_date",
                table: "InboundShipmentRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cargo_description",
                table: "InboundShipmentRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "branch_plant_id",
                table: "InboundShipmentRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "supplier_id",
                table: "InboundShipmentBLAWBs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "exchange_rate",
                table: "CurrencyExchange",
                type: "decimal(10,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,7)");

            migrationBuilder.AlterColumn<string>(
                name: "intercompany_flag",
                table: "Company",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "dormant_flag",
                table: "Company",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "received_date",
                table: "AsnHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "forecast_ex_wh_date",
                table: "AsnHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "estimated_putaway_date",
                table: "AsnHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "invoice_amt",
                table: "AsnHeaders",
                type: "decimal(9,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "invoice_currency_id",
                table: "AsnHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "invoice_exchange_rate",
                table: "AsnHeaders",
                type: "decimal(13,7)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "invoice_no",
                table: "AsnHeaders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "total_asn_amt",
                table: "AsnHeaders",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_gross_weight",
                table: "AsnHeaders",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_packages",
                table: "AsnHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "variance_reason",
                table: "AsnHeaders",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "volume",
                table: "AsnHeaders",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "extended_cost",
                table: "AsnDetails",
                type: "decimal(11,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "po_detail_id",
                table: "AsnDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "unit_cost",
                table: "AsnDetails",
                type: "decimal(8,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
