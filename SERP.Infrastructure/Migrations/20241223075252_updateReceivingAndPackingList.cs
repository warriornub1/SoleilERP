using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateReceivingAndPackingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "container_id",
                table: "ReceivingHeaders");

            migrationBuilder.DropColumn(
                name: "asn_detail_id",
                table: "ReceivingDetails");

            migrationBuilder.AddColumn<string>(
                name: "released_by",
                table: "ReceivingHeaders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            #region already exist at 20241223074058_AddPackingList
            //migrationBuilder.AddColumn<int>(
            //    name: "asn_header_id",
            //    table: "Containers",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "receiving_header_id",
            //    table: "Containers",
            //    type: "int",
            //    nullable: true); 
            #endregion

            migrationBuilder.CreateTable(
                name: "PackingLists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    container_id = table.Column<int>(type: "int", nullable: false),
                    asn_detail_id = table.Column<int>(type: "int", nullable: false),
                    package_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    mixed_carton_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    country_of_origin_id = table.Column<int>(type: "int", nullable: true),
                    no_of_carton = table.Column<int>(type: "int", nullable: true),
                    unit_per_carton = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingLists", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackingLists");

            migrationBuilder.DropColumn(
                name: "released_by",
                table: "ReceivingHeaders");

            #region already exist at 20241223074058_AddPackingList
            //migrationBuilder.DropColumn(
            //    name: "asn_header_id",
            //    table: "Containers");

            //migrationBuilder.DropColumn(
            //    name: "receiving_header_id",
            //    table: "Containers"); 
            #endregion

            migrationBuilder.AddColumn<int>(
                name: "container_id",
                table: "ReceivingHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "asn_detail_id",
                table: "ReceivingDetails",
                type: "int",
                nullable: true);
        }
    }
}
