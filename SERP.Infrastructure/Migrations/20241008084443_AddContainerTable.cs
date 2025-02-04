using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContainerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inbound_shipment_id = table.Column<int>(type: "int", nullable: true),
                    container_no = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    shipment_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    seal_no = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    weight = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Containers");
        }
    }
}
