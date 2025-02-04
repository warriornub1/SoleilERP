using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Employee_EmployeeStructureMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RevenueCenters_CompanyStructures_company_structure_id",
                table: "RevenueCenters");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    employee_no = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    employee_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ailas = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    occupation_description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStructureMappings_company_structure_id",
                table: "EmployeeStructureMappings",
                column: "company_structure_id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStructureMappings_employee_id_company_structure_id",
                table: "EmployeeStructureMappings",
                columns: new[] { "employee_id", "company_structure_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeStructureMappings_CompanyStructures_company_structure_id",
                table: "EmployeeStructureMappings",
                column: "company_structure_id",
                principalTable: "CompanyStructures",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeStructureMappings_Employees_employee_id",
                table: "EmployeeStructureMappings",
                column: "employee_id",
                principalTable: "Employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueCenters_CompanyStructures_company_structure_id",
                table: "RevenueCenters",
                column: "company_structure_id",
                principalTable: "CompanyStructures",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeStructureMappings_CompanyStructures_company_structure_id",
                table: "EmployeeStructureMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeStructureMappings_Employees_employee_id",
                table: "EmployeeStructureMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_RevenueCenters_CompanyStructures_company_structure_id",
                table: "RevenueCenters");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeStructureMappings_company_structure_id",
                table: "EmployeeStructureMappings");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeStructureMappings_employee_id_company_structure_id",
                table: "EmployeeStructureMappings");

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueCenters_CompanyStructures_company_structure_id",
                table: "RevenueCenters",
                column: "company_structure_id",
                principalTable: "CompanyStructures",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
