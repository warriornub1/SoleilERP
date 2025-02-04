using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_RevenueCenters_Costcenters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCenters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cost_center_code = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    cost_center_description = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    parent_group_id = table.Column<int>(type: "int", nullable: false),
                    status_flag = table.Column<int>(type: "int", maxLength: 1, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RevenueCenters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    revenue_center_code = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    revenue_center_description = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    parent_group_id = table.Column<int>(type: "int", nullable: false),
                    status_flag = table.Column<int>(type: "int", maxLength: 1, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueCenters", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_cost_center_code",
                table: "CostCenters",
                column: "cost_center_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RevenueCenters_revenue_center_code",
                table: "RevenueCenters",
                column: "revenue_center_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCenters");

            migrationBuilder.DropTable(
                name: "RevenueCenters");
        }
    }
}
