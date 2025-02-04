using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceForASN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exchange_rate",
                table: "InvoiceHeaders");

            migrationBuilder.DropColumn(
                name: "total_gross_weight",
                table: "InvoiceHeaders");

            migrationBuilder.DropColumn(
                name: "volume",
                table: "InvoiceHeaders");

            migrationBuilder.DropColumn(
                name: "item_id",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "notes_to_warehouse",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "status_flag",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "uom",
                table: "InvoiceDetails");

            migrationBuilder.RenameColumn(
                name: "total_packages",
                table: "InvoiceHeaders",
                newName: "receiving_header_id");

            migrationBuilder.RenameColumn(
                name: "unit_cost",
                table: "InvoiceDetails",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "extended_cost",
                table: "InvoiceDetails",
                newName: "total_price");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amt",
                table: "InvoiceHeaders",
                type: "decimal(13,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<string>(
                name: "invoice_no",
                table: "InvoiceHeaders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "asn_header_id",
                table: "InvoiceHeaders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "amt",
                table: "InvoiceHeaders",
                type: "decimal(11,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,2)");

            migrationBuilder.AddColumn<int>(
                name: "branch_plant_id",
                table: "InvoiceHeaders",
                type: "int",
                maxLength: 20,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "supplier_id",
                table: "InvoiceHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "exchange_rate",
                table: "InvoiceDetails",
                type: "decimal(13,7)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvoiceFiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_header_id = table.Column<int>(type: "int", nullable: false),
                    file_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceFiles", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceFiles");

            migrationBuilder.DropColumn(
                name: "branch_plant_id",
                table: "InvoiceHeaders");

            migrationBuilder.DropColumn(
                name: "supplier_id",
                table: "InvoiceHeaders");

            migrationBuilder.DropColumn(
                name: "exchange_rate",
                table: "InvoiceDetails");

            migrationBuilder.RenameColumn(
                name: "receiving_header_id",
                table: "InvoiceHeaders",
                newName: "total_packages");

            migrationBuilder.RenameColumn(
                name: "unit_price",
                table: "InvoiceDetails",
                newName: "unit_cost");

            migrationBuilder.RenameColumn(
                name: "total_price",
                table: "InvoiceDetails",
                newName: "extended_cost");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amt",
                table: "InvoiceHeaders",
                type: "decimal(11,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,4)");

            migrationBuilder.AlterColumn<string>(
                name: "invoice_no",
                table: "InvoiceHeaders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "asn_header_id",
                table: "InvoiceHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "amt",
                table: "InvoiceHeaders",
                type: "decimal(9,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,4)");

            migrationBuilder.AddColumn<decimal>(
                name: "exchange_rate",
                table: "InvoiceHeaders",
                type: "decimal(13,7)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_gross_weight",
                table: "InvoiceHeaders",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "volume",
                table: "InvoiceHeaders",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "item_id",
                table: "InvoiceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "notes_to_warehouse",
                table: "InvoiceDetails",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status_flag",
                table: "InvoiceDetails",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "uom",
                table: "InvoiceDetails",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);
        }
    }
}
