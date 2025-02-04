using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_header_id = table.Column<int>(type: "int", nullable: false),
                    line_no = table.Column<int>(type: "int", nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    po_detail_id = table.Column<int>(type: "int", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    unit_cost = table.Column<decimal>(type: "decimal(11,4)", nullable: false),
                    extended_cost = table.Column<decimal>(type: "decimal(13,4)", nullable: false),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: true),
                    notes_to_warehouse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceHeaders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asn_header_id = table.Column<int>(type: "int", nullable: false),
                    invoice_no = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    amt = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    total_amt = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    exchange_rate = table.Column<decimal>(type: "decimal(13,7)", nullable: true),
                    total_packages = table.Column<int>(type: "int", nullable: true),
                    total_gross_weight = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    volume = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    variance_reason = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceHeaders", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "InvoiceHeaders");
        }
    }
}
