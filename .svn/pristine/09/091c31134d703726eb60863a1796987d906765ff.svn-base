using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_CompanyStruct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyStructures_org_type_sequence",
                table: "CompanyStructures");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_org_type_sequence_parent_id",
                table: "CompanyStructures",
                columns: new[] { "org_type", "sequence", "parent_id" },
                unique: true,
                filter: "[parent_id] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyStructures_org_type_sequence_parent_id",
                table: "CompanyStructures");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_org_type_sequence",
                table: "CompanyStructures",
                columns: new[] { "org_type", "sequence" },
                unique: true);
        }
    }
}
