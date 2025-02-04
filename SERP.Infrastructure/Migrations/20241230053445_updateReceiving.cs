using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateReceiving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "released_by",
                table: "ReceivingHeaders");

            //migrationBuilder.RenameColumn(
            //    name: "received_qty",
            //    table: "AsnDetails",
            //    newName: "po_detail_id");

            migrationBuilder.AddColumn<int>(
                name: "asn_header_id",
                table: "ReceivingHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "inspection_due_date",
                table: "ReceivingHeaders",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "asn_header_id",
                table: "ReceivingHeaders");

            migrationBuilder.DropColumn(
                name: "inspection_due_date",
                table: "ReceivingHeaders");

            //migrationBuilder.RenameColumn(
            //    name: "po_detail_id",
            //    table: "AsnDetails",
            //    newName: "received_qty");

            migrationBuilder.AddColumn<string>(
                name: "released_by",
                table: "ReceivingHeaders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
