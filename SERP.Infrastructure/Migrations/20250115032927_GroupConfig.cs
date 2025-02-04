using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GroupConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Group_parent_group_id",
                table: "Group");

            migrationBuilder.DropForeignKey(
                name: "FK_NaturalAccounts_Group_parent_group_id",
                table: "NaturalAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "status_flag",
                table: "Group",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "E",
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Group_GroupType",
                table: "Group",
                sql: "group_type IN ('CO', 'NA', 'CC', 'RC')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Group_StatusFlag",
                table: "Group",
                sql: "status_flag IN ('E', 'D')");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Group_parent_group_id",
                table: "Group",
                column: "parent_group_id",
                principalTable: "Group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Group_Group_parent_group_id",
                table: "Group");

            migrationBuilder.DropForeignKey(
                name: "FK_NaturalAccounts_Group_parent_group_id",
                table: "NaturalAccounts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Group_GroupType",
                table: "Group");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Group_StatusFlag",
                table: "Group");

            migrationBuilder.AlterColumn<string>(
                name: "status_flag",
                table: "Group",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldDefaultValue: "E");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Group_parent_group_id",
                table: "Group",
                column: "parent_group_id",
                principalTable: "Group",
                principalColumn: "id");

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
