using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_RevenueCenter_Constraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCenterCompanyMapping");

            migrationBuilder.DropTable(
                name: "RevenueCenterCompanyMapping");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_company_structure_id",
                table: "CostCenters");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueCenters_company_structure_id",
                table: "RevenueCenters",
                column: "company_structure_id",
                unique: true,
                filter: "[company_structure_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueCenters_parent_group_id",
                table: "RevenueCenters",
                column: "parent_group_id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CostCenter_StatusFlag1",
                table: "RevenueCenters",
                sql: "status_flag IN ('E', 'D')");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_company_structure_id",
                table: "CostCenters",
                column: "company_structure_id",
                unique: true,
                filter: "[company_structure_id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueCenters_CompanyStructures_company_structure_id",
                table: "RevenueCenters",
                column: "company_structure_id",
                principalTable: "CompanyStructures",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueCenters_Group_parent_group_id",
                table: "RevenueCenters",
                column: "parent_group_id",
                principalTable: "Group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RevenueCenters_CompanyStructures_company_structure_id",
                table: "RevenueCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_RevenueCenters_Group_parent_group_id",
                table: "RevenueCenters");

            migrationBuilder.DropIndex(
                name: "IX_RevenueCenters_company_structure_id",
                table: "RevenueCenters");

            migrationBuilder.DropIndex(
                name: "IX_RevenueCenters_parent_group_id",
                table: "RevenueCenters");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CostCenter_StatusFlag1",
                table: "RevenueCenters");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_company_structure_id",
                table: "CostCenters");

            migrationBuilder.CreateTable(
                name: "CostCenterCompanyMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    cost_center_id = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenterCompanyMapping", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RevenueCenterCompanyMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    revenue_center_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueCenterCompanyMapping", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_company_structure_id",
                table: "CostCenters",
                column: "company_structure_id");
        }
    }
}
