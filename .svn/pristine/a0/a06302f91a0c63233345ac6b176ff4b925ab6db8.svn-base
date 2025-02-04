using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCostCenterConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "company_structure_id",
                table: "CostCenters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_company_structure_id",
                table: "CostCenters",
                column: "company_structure_id");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_parent_group_id",
                table: "CostCenters",
                column: "parent_group_id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CostCenter_StatusFlag",
                table: "CostCenters",
                sql: "status_flag IN ('E', 'D')");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_id_company_id",
                table: "CompanyStructures",
                columns: new[] { "id", "company_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyStructures_CostCenters_company_id",
                table: "CompanyStructures",
                column: "company_id",
                principalTable: "CostCenters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenters_CompanyStructures_company_structure_id",
                table: "CostCenters",
                column: "company_structure_id",
                principalTable: "CompanyStructures",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenters_Group_parent_group_id",
                table: "CostCenters",
                column: "parent_group_id",
                principalTable: "Group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyStructures_CostCenters_company_id",
                table: "CompanyStructures");

            migrationBuilder.DropForeignKey(
                name: "FK_CostCenters_CompanyStructures_company_structure_id",
                table: "CostCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_CostCenters_Group_parent_group_id",
                table: "CostCenters");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_company_structure_id",
                table: "CostCenters");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_parent_group_id",
                table: "CostCenters");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CostCenter_StatusFlag",
                table: "CostCenters");

            migrationBuilder.DropIndex(
                name: "IX_CompanyStructures_id_company_id",
                table: "CompanyStructures");

            migrationBuilder.DropColumn(
                name: "company_structure_id",
                table: "CostCenters");
        }
    }
}
