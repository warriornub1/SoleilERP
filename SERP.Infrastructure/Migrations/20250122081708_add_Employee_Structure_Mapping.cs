using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Employee_Structure_Mapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCenters_CompanyStructures_company_structure_id",
                table: "CostCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_RevenueCenters_Group_parent_group_id",
                table: "RevenueCenters");

            migrationBuilder.AddColumn<int>(
                name: "in_charge_employee_id",
                table: "CompanyStructures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "top_flag",
                table: "CompanyStructures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmployeeStructureMappings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    company_structure_id = table.Column<int>(type: "int", nullable: false),
                    core_flag = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeStructureMappings", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenters_CompanyStructures_company_structure_id",
                table: "CostCenters",
                column: "company_structure_id",
                principalTable: "CompanyStructures",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueCenters_Group_parent_group_id",
                table: "RevenueCenters",
                column: "parent_group_id",
                principalTable: "Group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCenters_CompanyStructures_company_structure_id",
                table: "CostCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_RevenueCenters_Group_parent_group_id",
                table: "RevenueCenters");

            migrationBuilder.DropTable(
                name: "EmployeeStructureMappings");

            migrationBuilder.DropColumn(
                name: "in_charge_employee_id",
                table: "CompanyStructures");

            migrationBuilder.DropColumn(
                name: "top_flag",
                table: "CompanyStructures");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenters_CompanyStructures_company_structure_id",
                table: "CostCenters",
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
    }
}
