using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPackingListAndReceivingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackingListDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    packing_list_header_id = table.Column<int>(type: "int", nullable: false),
                    asn_detail_id = table.Column<int>(type: "int", nullable: false),
                    receiving_detail_id = table.Column<int>(type: "int", nullable: false),
                    package_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mixed_carton_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    uom = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: false),
                    no_of_carton = table.Column<int>(type: "int", nullable: false),
                    unit_per_carton = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingListDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PackingListHeaders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    packing_list_no = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    asn_header_id = table.Column<int>(type: "int", nullable: false),
                    container_id = table.Column<int>(type: "int", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingListHeaders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receiving_header_id = table.Column<int>(type: "int", nullable: false),
                    line_no = table.Column<int>(type: "int", nullable: false),
                    po_detail_id = table.Column<int>(type: "int", nullable: true),
                    asn_detail_id = table.Column<int>(type: "int", nullable: true),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    inspected_qty = table.Column<int>(type: "int", nullable: false),
                    uom = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    po_uom = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    package_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: true),
                    supplier_document_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    supplier_document_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingFiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receiving_header_id = table.Column<int>(type: "int", nullable: false),
                    receiving_detail_id = table.Column<int>(type: "int", nullable: false),
                    file_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingFiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingHeaders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receiving_no = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    branch_plant_id = table.Column<int>(type: "int", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: true),
                    receiving_from_document_type = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    container_id = table.Column<int>(type: "int", nullable: false),
                    received_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    inspector_assigned_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    inspection_start_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    inspection_end_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    inspected_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingHeaders", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackingListDetails");

            migrationBuilder.DropTable(
                name: "PackingListHeaders");

            migrationBuilder.DropTable(
                name: "ReceivingDetails");

            migrationBuilder.DropTable(
                name: "ReceivingFiles");

            migrationBuilder.DropTable(
                name: "ReceivingHeaders");
        }
    }
}
