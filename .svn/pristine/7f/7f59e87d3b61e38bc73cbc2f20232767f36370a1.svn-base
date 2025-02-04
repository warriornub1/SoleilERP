using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContainerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "seal_no",
                table: "Containers");

            migrationBuilder.AddColumn<DateTime>(
                name: "detention_date",
                table: "Containers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "no_of_packages",
                table: "Containers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "no_of_packages_unloaded",
                table: "Containers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "received_by",
                table: "Containers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "received_on",
                table: "Containers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "released_by",
                table: "Containers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "released_on",
                table: "Containers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "unload_end_on",
                table: "Containers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unload_remark",
                table: "Containers",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "unload_start_on",
                table: "Containers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unloaded_by",
                table: "Containers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "detention_date",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "no_of_packages",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "no_of_packages_unloaded",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "received_by",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "received_on",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "released_by",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "released_on",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "unload_end_on",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "unload_remark",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "unload_start_on",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "unloaded_by",
                table: "Containers");

            migrationBuilder.AddColumn<string>(
                name: "seal_no",
                table: "Containers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
