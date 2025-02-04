using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Group_GroupCode_GroupType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Group_group_code",
                table: "Group");

            migrationBuilder.CreateIndex(
                name: "IX_Group_group_code_group_type",
                table: "Group",
                columns: new[] { "group_code", "group_type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Group_group_code_group_type",
                table: "Group");

            migrationBuilder.CreateIndex(
                name: "IX_Group_group_code",
                table: "Group",
                column: "group_code",
                unique: true);
        }
    }
}
