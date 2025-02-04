using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyStructures",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    sequence = table.Column<int>(type: "int", nullable: false),
                    org_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    org_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    org_description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    org_type = table.Column<int>(type: "int", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStructures", x => x.id);
                    table.CheckConstraint("CK_CompanyStructure_OrgType", "org_type IN ('1', '2', '3', '4')");
                    table.CheckConstraint("CK_CompanyStructure_StatusFlag", "status_flag IN ('E', 'D')");
                    table.ForeignKey(
                        name: "FK_CompanyStructures_CompanyStructures_parent_id",
                        column: x => x.parent_id,
                        principalTable: "CompanyStructures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyStructures_Company_company_id",
                        column: x => x.company_id,
                        principalTable: "Company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_company_id",
                table: "CompanyStructures",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_org_type_sequence",
                table: "CompanyStructures",
                columns: new[] { "org_type", "sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_parent_id",
                table: "CompanyStructures",
                column: "parent_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyStructures");
        }
    }
}
