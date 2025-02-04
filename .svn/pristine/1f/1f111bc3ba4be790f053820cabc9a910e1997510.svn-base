using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSecondarySupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "supplier_no",
                table: "SecondarySupplier",
                newName: "sec_supplier_no");

            migrationBuilder.RenameColumn(
                name: "supplier_name",
                table: "SecondarySupplier",
                newName: "sec_supplier_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sec_supplier_no",
                table: "SecondarySupplier",
                newName: "supplier_no");

            migrationBuilder.RenameColumn(
                name: "sec_supplier_name",
                table: "SecondarySupplier",
                newName: "supplier_name");
        }
    }
}
