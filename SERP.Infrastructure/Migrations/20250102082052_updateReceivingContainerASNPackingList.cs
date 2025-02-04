using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateReceivingContainerASNPackingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackingLists");

            migrationBuilder.DropColumn(
                name: "asn_header_id",
                table: "ReceivingHeaders");

            migrationBuilder.DropColumn(
                name: "asn_header_id",
                table: "Containers");

            migrationBuilder.RenameColumn(
                name: "shipment_type",
                table: "Containers",
                newName: "container_type");

            migrationBuilder.AddColumn<int>(
                name: "asn_detail_id",
                table: "ReceivingDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "packing_header_id",
                table: "ReceivingDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContainerASNs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asn_header_id = table.Column<int>(type: "int", nullable: false),
                    container_id = table.Column<int>(type: "int", nullable: false),
                    packing_list_no = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerASNs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PackingDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    packing_header_id = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_PackingDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PackingHeaders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    container_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingHeaders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingASNs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receiving_HEADER_id = table.Column<int>(type: "int", nullable: false),
                    asn_header_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingASNs", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContainerASNs");

            migrationBuilder.DropTable(
                name: "PackingDetails");

            migrationBuilder.DropTable(
                name: "PackingHeaders");

            migrationBuilder.DropTable(
                name: "ReceivingASNs");

            migrationBuilder.DropColumn(
                name: "asn_detail_id",
                table: "ReceivingDetails");

            migrationBuilder.DropColumn(
                name: "packing_header_id",
                table: "ReceivingDetails");

            migrationBuilder.RenameColumn(
                name: "container_type",
                table: "Containers",
                newName: "shipment_type");

            migrationBuilder.AddColumn<int>(
                name: "asn_header_id",
                table: "ReceivingHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "asn_header_id",
                table: "Containers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PackingLists",
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
                    table.PrimaryKey("PK_PackingLists", x => x.id);
                });
        }
    }
}
