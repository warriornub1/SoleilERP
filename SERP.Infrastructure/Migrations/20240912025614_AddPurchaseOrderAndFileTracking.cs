using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseOrderAndFileTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilesTracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    upload_source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    url_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    file_size = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesTracking", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PoDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po_header_id = table.Column<int>(type: "int", nullable: false),
                    line_no = table.Column<int>(type: "int", nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    po_item_description = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    uom = table.Column<int>(type: "int", nullable: false),
                    supplier_item_mapping_id = table.Column<int>(type: "int", nullable: false),
                    ship_to_bp_id = table.Column<int>(type: "int", nullable: false),
                    unit_cost = table.Column<int>(type: "int", nullable: false),
                    line_discount = table.Column<int>(type: "int", nullable: false),
                    cost_rule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    secondary_supplier_id = table.Column<int>(type: "int", nullable: false),
                    supplier_acknowledgement_no = table.Column<int>(type: "int", nullable: true),
                    instruction_to_supplier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    internal_reference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    notes_to_warehouse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    requested_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    quoted_ex_fac_date_earliest = table.Column<DateTime>(type: "datetime2", nullable: true),
                    quoted_ex_fac_date_latest = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ack_ex_fac_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    forecast_ex_wh_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    collection_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PoFiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po_header_id = table.Column<int>(type: "int", nullable: false),
                    file_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoFiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PoHeaders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po_no = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    po_type = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    po_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    business_unit_id = table.Column<int>(type: "int", nullable: false),
                    issuing_branch_plant_id = table.Column<int>(type: "int", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    intermediary_supplier_id = table.Column<int>(type: "int", nullable: false),
                    secondary_supplier_id = table.Column<int>(type: "int", nullable: false),
                    ship_to_branch_plant_id = table.Column<int>(type: "int", nullable: false),
                    ship_to_site_id = table.Column<int>(type: "int", nullable: false),
                    forwarder_id = table.Column<int>(type: "int", nullable: false),
                    sales_order_id = table.Column<int>(type: "int", nullable: false),
                    payment_term = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    base_currency_id = table.Column<int>(type: "int", nullable: false),
                    po_currency_id = table.Column<int>(type: "int", nullable: false),
                    exchange_rate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    cost_rule = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    urgency_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    order_discount = table.Column<decimal>(type: "decimal(9,2)", nullable: true),
                    taken_by = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    internal_remarks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    freight_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    self_collect_site_id = table.Column<int>(type: "int", nullable: false),
                    port_of_discharge_id = table.Column<int>(type: "int", nullable: false),
                    send_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    quotation_reference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    supplier_acknowledgement_no = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    supplier_marking_reference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    notes_to_supplier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    requested_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    quoted_ex_fac_date_earliest = table.Column<DateTime>(type: "datetime2", nullable: true),
                    quoted_ex_fac_date_latest = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ack_ex_fac_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    forecast_ex_wh_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    collection_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    delivery_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoHeaders", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilesTracking");

            migrationBuilder.DropTable(
                name: "PoDetails");

            migrationBuilder.DropTable(
                name: "PoFiles");

            migrationBuilder.DropTable(
                name: "PoHeaders");
        }
    }
}
