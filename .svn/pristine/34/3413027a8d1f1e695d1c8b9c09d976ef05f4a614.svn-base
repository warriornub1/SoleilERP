using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomView",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    custom_view_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    custom_view_name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    full_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    default_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    private_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    status_flag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomView", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CustomViewAttributes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    custom_view_id = table.Column<int>(type: "int", nullable: false),
                    attribute = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    attribute_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    seq_no = table.Column<int>(type: "int", nullable: false),
                    column_freeze_flag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomViewAttributes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomView");

            migrationBuilder.DropTable(
                name: "CustomViewAttributes");
        }
    }
}
