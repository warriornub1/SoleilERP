using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Natural_Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NaturalAccounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    natural_account_code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    natural_account_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    natural_account_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    parent_group_id = table.Column<int>(type: "int", nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalAccounts", x => x.id);
                    table.CheckConstraint("CK_NaturalAccount_NaturalAccountType", "natural_account_type IN ('AS', 'LB', 'SE', 'PL')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NaturalAccounts_natural_account_code",
                table: "NaturalAccounts",
                column: "natural_account_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NaturalAccounts");
        }
    }
}
