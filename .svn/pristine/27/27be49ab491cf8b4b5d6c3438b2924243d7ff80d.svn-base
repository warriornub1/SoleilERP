using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "inbound_shipment_id",
                table: "Containers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierItemMapping_supplier_id_item_id_supplier_part_no",
                table: "SupplierItemMapping",
                columns: new[] { "supplier_id", "item_id", "supplier_part_no" });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierGroups_group_supplier_no",
                table: "SupplierGroups",
                column: "group_supplier_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_supplier_no",
                table: "Supplier",
                column: "supplier_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Site_site_no",
                table: "Site",
                column: "site_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ports_port_no",
                table: "Ports",
                column: "port_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoHeaders_po_no",
                table: "PoHeaders",
                column: "po_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_item_no",
                table: "Item",
                column: "item_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InboundShipments_inbound_shipment_no",
                table: "InboundShipments",
                column: "inbound_shipment_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_customer_no",
                table: "Customer",
                column: "customer_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_country_alpha_code_two_country_alpha_code_three",
                table: "Country",
                columns: new[] { "country_alpha_code_two", "country_alpha_code_three" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Containers_inbound_shipment_id_container_no",
                table: "Containers",
                columns: new[] { "inbound_shipment_id", "container_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_company_no",
                table: "Company",
                column: "company_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BranchPlant_branch_plant_no",
                table: "BranchPlant",
                column: "branch_plant_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsnHeaders_asn_no",
                table: "AsnHeaders",
                column: "asn_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agent_agent_no",
                table: "Agent",
                column: "agent_no",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SupplierItemMapping_supplier_id_item_id_supplier_part_no",
                table: "SupplierItemMapping");

            migrationBuilder.DropIndex(
                name: "IX_SupplierGroups_group_supplier_no",
                table: "SupplierGroups");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_supplier_no",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Site_site_no",
                table: "Site");

            migrationBuilder.DropIndex(
                name: "IX_Ports_port_no",
                table: "Ports");

            migrationBuilder.DropIndex(
                name: "IX_PoHeaders_po_no",
                table: "PoHeaders");

            migrationBuilder.DropIndex(
                name: "IX_Item_item_no",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_InboundShipments_inbound_shipment_no",
                table: "InboundShipments");

            migrationBuilder.DropIndex(
                name: "IX_Customer_customer_no",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Country_country_alpha_code_two_country_alpha_code_three",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Containers_inbound_shipment_id_container_no",
                table: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_Company_company_no",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_BranchPlant_branch_plant_no",
                table: "BranchPlant");

            migrationBuilder.DropIndex(
                name: "IX_AsnHeaders_asn_no",
                table: "AsnHeaders");

            migrationBuilder.DropIndex(
                name: "IX_Agent_agent_no",
                table: "Agent");

            migrationBuilder.AlterColumn<int>(
                name: "inbound_shipment_id",
                table: "Containers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
