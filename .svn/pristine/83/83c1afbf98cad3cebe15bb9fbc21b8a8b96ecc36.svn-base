using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCostCenterConfig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyStructures_CostCenters_company_id",
                table: "CompanyStructures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_CompanyStructures_CostCenters_company_id",
                table: "CompanyStructures",
                column: "company_id",
                principalTable: "CostCenters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
