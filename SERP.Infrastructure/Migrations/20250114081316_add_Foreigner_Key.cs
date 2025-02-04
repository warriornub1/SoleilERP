using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Foreigner_Key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_NaturalAccounts_parent_group_id",
                table: "NaturalAccounts",
                column: "parent_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_NaturalAccounts_Group_parent_group_id",
                table: "NaturalAccounts",
                column: "parent_group_id",
                principalTable: "Group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NaturalAccounts_Group_parent_group_id",
                table: "NaturalAccounts");

            migrationBuilder.DropIndex(
                name: "IX_NaturalAccounts_parent_group_id",
                table: "NaturalAccounts");
        }
    }
}
