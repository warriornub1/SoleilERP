using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    description_1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description_2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    primary_uom = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    secondary_uom = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    purchasing_uom = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    purchase_min_order_qty = table.Column<int>(type: "int", nullable: false),
                    purchase_multiple_order_qty = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Lov",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lov_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    lov_value = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    lov_label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lov", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SecondarySupplier",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    supplier_no = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    supplier_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondarySupplier", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_no = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    supplier_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    service_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    product_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    authorised_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    payment_term = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    payment_method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    default_currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    landed_cost_rule = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    inco_term = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    default_freight_method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    po_sending_method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    intermediary_supplier_no = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierItemMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    supplier_part_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    supplier_material_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    supplier_material_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    default_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierItemMapping", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Lov");

            migrationBuilder.DropTable(
                name: "SecondarySupplier");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "SupplierItemMapping");
        }
    }
}
