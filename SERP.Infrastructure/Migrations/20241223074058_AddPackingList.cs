using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPackingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Containers_inbound_shipment_blawb_id_container_no",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "inbound_shipment_id",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "inbound_shipment_request_id",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "received_qty",
                table: "AsnDetails");

            migrationBuilder.RenameColumn(
                name: "inbound_shipment_blawb_id",
                table: "Containers",
                newName: "receiving_header_id");

            migrationBuilder.RenameColumn(
                name: "shipment_arranged_supplier_flag",
                table: "AsnHeaders",
                newName: "notes_to_warehouse_flag");

            migrationBuilder.AddColumn<int>(
                name: "asn_header_id",
                table: "Containers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "attachment_flag",
                table: "AsnHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "po_detail_id",
                table: "AsnDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PackingList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    container_id = table.Column<int>(type: "int", nullable: false),
                    asn_detail_id = table.Column<int>(type: "int", nullable: false),
                    package_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    mixed_carton_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: true),
                    no_of_carton = table.Column<int>(type: "int", nullable: true),
                    unit_per_carton = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingList", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_container_no",
                table: "Containers",
                column: "container_no");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackingList");

            migrationBuilder.DropIndex(
                name: "IX_Containers_container_no",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "asn_header_id",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "attachment_flag",
                table: "AsnHeaders");

            migrationBuilder.DropColumn(
                name: "po_detail_id",
                table: "AsnDetails");

            migrationBuilder.RenameColumn(
                name: "receiving_header_id",
                table: "Containers",
                newName: "inbound_shipment_blawb_id");

            migrationBuilder.RenameColumn(
                name: "notes_to_warehouse_flag",
                table: "AsnHeaders",
                newName: "shipment_arranged_supplier_flag");

            migrationBuilder.AddColumn<int>(
                name: "inbound_shipment_id",
                table: "AsnHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "inbound_shipment_request_id",
                table: "AsnHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "received_qty",
                table: "AsnDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Containers_inbound_shipment_blawb_id_container_no",
                table: "Containers",
                columns: new[] { "inbound_shipment_blawb_id", "container_no" },
                unique: true,
                filter: "[inbound_shipment_blawb_id] IS NOT NULL");
        }
    }
}
