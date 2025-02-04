using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removePackingListHeaderAndDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackingListDetails");

            migrationBuilder.DropTable(
                name: "PackingListHeaders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackingListDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    mixed_carton_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    no_of_carton = table.Column<int>(type: "int", nullable: true),
                    package_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    packing_list_header_id = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    receiving_detail_id = table.Column<int>(type: "int", nullable: true),
                    unit_per_carton = table.Column<int>(type: "int", nullable: true),
                    uom = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
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
                    container_id = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    packing_list_no = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingListHeaders", x => x.id);
                });
        }
    }
}
