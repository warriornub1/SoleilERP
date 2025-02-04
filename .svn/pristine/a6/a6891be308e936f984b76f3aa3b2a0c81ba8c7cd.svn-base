using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class prevent_deletion_of_parent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NaturalAccounts_Group_parent_group_id",
                table: "NaturalAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_NaturalAccounts_Group_parent_group_id",
                table: "NaturalAccounts",
                column: "parent_group_id",
                principalTable: "Group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NaturalAccounts_Group_parent_group_id",
                table: "NaturalAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_NaturalAccounts_Group_parent_group_id",
                table: "NaturalAccounts",
                column: "parent_group_id",
                principalTable: "Group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
