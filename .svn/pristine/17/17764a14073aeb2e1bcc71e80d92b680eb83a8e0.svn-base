using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddASNPackingDiscrepancyAndUpdatePackingHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ASNPackingList");

            migrationBuilder.DropTable(
                name: "PackingList");

            migrationBuilder.AddColumn<int>(
                name: "asn_header_id",
                table: "PackingHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "po_detail_id",
                table: "AsnDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ASNPackingDiscrepancy",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asn_header_id = table.Column<int>(type: "int", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: true),
                    asn_qty = table.Column<int>(type: "int", nullable: false),
                    packing_list_qty = table.Column<int>(type: "int", nullable: false),
                    uom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASNPackingDiscrepancy", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ASNPackingDiscrepancy");

            migrationBuilder.DropColumn(
                name: "asn_header_id",
                table: "PackingHeaders");

            migrationBuilder.AlterColumn<int>(
                name: "po_detail_id",
                table: "AsnDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ASNPackingList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asn_header_id = table.Column<int>(type: "int", nullable: false),
                    asn_qty = table.Column<int>(type: "int", nullable: false),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    packing_list_qty = table.Column<int>(type: "int", nullable: false),
                    uom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASNPackingList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PackingList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asn_detail_id = table.Column<int>(type: "int", nullable: false),
                    container_id = table.Column<int>(type: "int", nullable: false),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    mixed_carton_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    no_of_carton = table.Column<int>(type: "int", nullable: true),
                    package_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    qty = table.Column<int>(type: "int", nullable: false),
                    unit_per_carton = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingList", x => x.id);
                });
        }
    }
}
