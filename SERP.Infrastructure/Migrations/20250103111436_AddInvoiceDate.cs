using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<DateOnly>(
                name: "invoice_date",
                table: "InvoiceHeaders",
                type: "date",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "detention_date",
                table: "Containers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "po_detail_id",
                table: "AsnDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "invoice_date",
                table: "InvoiceHeaders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "detention_date",
                table: "Containers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "po_detail_id",
                table: "AsnDetails",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

        }
    }
}
