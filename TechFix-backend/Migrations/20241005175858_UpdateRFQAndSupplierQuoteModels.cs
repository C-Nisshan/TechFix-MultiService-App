using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechFix_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRFQAndSupplierQuoteModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Quotes_QuoteId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteItems_ProductRefs_ProductId",
                table: "QuoteItems");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteItems_Quotes_SupplierQuoteId",
                table: "QuoteItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_RFQ_RFQId",
                table: "Quotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Suppliers_SupplierId",
                table: "Quotes");

            migrationBuilder.DropForeignKey(
                name: "FK_RFQItem_ProductRefs_ProductId",
                table: "RFQItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RFQItem_RFQ_RFQId",
                table: "RFQItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RFQItem",
                table: "RFQItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RFQ",
                table: "RFQ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteItems",
                table: "QuoteItems");

            migrationBuilder.RenameTable(
                name: "RFQItem",
                newName: "RFQItems");

            migrationBuilder.RenameTable(
                name: "RFQ",
                newName: "RFQs");

            migrationBuilder.RenameTable(
                name: "Quotes",
                newName: "SupplierQuotes");

            migrationBuilder.RenameTable(
                name: "QuoteItems",
                newName: "SupplierQuoteItems");

            migrationBuilder.RenameIndex(
                name: "IX_RFQItem_RFQId",
                table: "RFQItems",
                newName: "IX_RFQItems_RFQId");

            migrationBuilder.RenameIndex(
                name: "IX_RFQItem_ProductId",
                table: "RFQItems",
                newName: "IX_RFQItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Quotes_SupplierId",
                table: "SupplierQuotes",
                newName: "IX_SupplierQuotes_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Quotes_RFQId",
                table: "SupplierQuotes",
                newName: "IX_SupplierQuotes_RFQId");

            migrationBuilder.RenameIndex(
                name: "IX_QuoteItems_SupplierQuoteId",
                table: "SupplierQuoteItems",
                newName: "IX_SupplierQuoteItems_SupplierQuoteId");

            migrationBuilder.RenameIndex(
                name: "IX_QuoteItems_ProductId",
                table: "SupplierQuoteItems",
                newName: "IX_SupplierQuoteItems_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "ProductRefProductId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductRefProductId",
                table: "Inventories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierQuoteId",
                table: "RFQItems",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "RFQs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "RFQs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductRefProductId",
                table: "SupplierQuoteItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFQItems",
                table: "RFQItems",
                column: "RFQItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFQs",
                table: "RFQs",
                column: "RFQId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupplierQuotes",
                table: "SupplierQuotes",
                column: "SupplierQuoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupplierQuoteItems",
                table: "SupplierQuoteItems",
                column: "SupplierQuoteItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductRefProductId",
                table: "OrderItems",
                column: "ProductRefProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductRefProductId",
                table: "Inventories",
                column: "ProductRefProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RFQItems_SupplierQuoteId",
                table: "RFQItems",
                column: "SupplierQuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_RFQs_SupplierId",
                table: "RFQs",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierQuoteItems_ProductRefProductId",
                table: "SupplierQuoteItems",
                column: "ProductRefProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_ProductRefs_ProductRefProductId",
                table: "Inventories",
                column: "ProductRefProductId",
                principalTable: "ProductRefs",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductRefs_ProductRefProductId",
                table: "OrderItems",
                column: "ProductRefProductId",
                principalTable: "ProductRefs",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_SupplierQuotes_QuoteId",
                table: "Orders",
                column: "QuoteId",
                principalTable: "SupplierQuotes",
                principalColumn: "SupplierQuoteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RFQItems_ProductRefs_ProductId",
                table: "RFQItems",
                column: "ProductId",
                principalTable: "ProductRefs",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RFQItems_RFQs_RFQId",
                table: "RFQItems",
                column: "RFQId",
                principalTable: "RFQs",
                principalColumn: "RFQId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RFQItems_SupplierQuotes_SupplierQuoteId",
                table: "RFQItems",
                column: "SupplierQuoteId",
                principalTable: "SupplierQuotes",
                principalColumn: "SupplierQuoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_RFQs_Suppliers_SupplierId",
                table: "RFQs",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierQuoteItems_ProductRefs_ProductId",
                table: "SupplierQuoteItems",
                column: "ProductId",
                principalTable: "ProductRefs",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierQuoteItems_ProductRefs_ProductRefProductId",
                table: "SupplierQuoteItems",
                column: "ProductRefProductId",
                principalTable: "ProductRefs",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierQuoteItems_SupplierQuotes_SupplierQuoteId",
                table: "SupplierQuoteItems",
                column: "SupplierQuoteId",
                principalTable: "SupplierQuotes",
                principalColumn: "SupplierQuoteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierQuotes_RFQs_RFQId",
                table: "SupplierQuotes",
                column: "RFQId",
                principalTable: "RFQs",
                principalColumn: "RFQId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierQuotes_Suppliers_SupplierId",
                table: "SupplierQuotes",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_ProductRefs_ProductRefProductId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductRefs_ProductRefProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_SupplierQuotes_QuoteId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RFQItems_ProductRefs_ProductId",
                table: "RFQItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RFQItems_RFQs_RFQId",
                table: "RFQItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RFQItems_SupplierQuotes_SupplierQuoteId",
                table: "RFQItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RFQs_Suppliers_SupplierId",
                table: "RFQs");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierQuoteItems_ProductRefs_ProductId",
                table: "SupplierQuoteItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierQuoteItems_ProductRefs_ProductRefProductId",
                table: "SupplierQuoteItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierQuoteItems_SupplierQuotes_SupplierQuoteId",
                table: "SupplierQuoteItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierQuotes_RFQs_RFQId",
                table: "SupplierQuotes");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierQuotes_Suppliers_SupplierId",
                table: "SupplierQuotes");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductRefProductId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_ProductRefProductId",
                table: "Inventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupplierQuotes",
                table: "SupplierQuotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupplierQuoteItems",
                table: "SupplierQuoteItems");

            migrationBuilder.DropIndex(
                name: "IX_SupplierQuoteItems_ProductRefProductId",
                table: "SupplierQuoteItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RFQs",
                table: "RFQs");

            migrationBuilder.DropIndex(
                name: "IX_RFQs_SupplierId",
                table: "RFQs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RFQItems",
                table: "RFQItems");

            migrationBuilder.DropIndex(
                name: "IX_RFQItems_SupplierQuoteId",
                table: "RFQItems");

            migrationBuilder.DropColumn(
                name: "ProductRefProductId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductRefProductId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "ProductRefProductId",
                table: "SupplierQuoteItems");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "RFQs");

            migrationBuilder.DropColumn(
                name: "SupplierQuoteId",
                table: "RFQItems");

            migrationBuilder.RenameTable(
                name: "SupplierQuotes",
                newName: "Quotes");

            migrationBuilder.RenameTable(
                name: "SupplierQuoteItems",
                newName: "QuoteItems");

            migrationBuilder.RenameTable(
                name: "RFQs",
                newName: "RFQ");

            migrationBuilder.RenameTable(
                name: "RFQItems",
                newName: "RFQItem");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierQuotes_SupplierId",
                table: "Quotes",
                newName: "IX_Quotes_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierQuotes_RFQId",
                table: "Quotes",
                newName: "IX_Quotes_RFQId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierQuoteItems_SupplierQuoteId",
                table: "QuoteItems",
                newName: "IX_QuoteItems_SupplierQuoteId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierQuoteItems_ProductId",
                table: "QuoteItems",
                newName: "IX_QuoteItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_RFQItems_RFQId",
                table: "RFQItem",
                newName: "IX_RFQItem_RFQId");

            migrationBuilder.RenameIndex(
                name: "IX_RFQItems_ProductId",
                table: "RFQItem",
                newName: "IX_RFQItem_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RFQ",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes",
                column: "SupplierQuoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteItems",
                table: "QuoteItems",
                column: "SupplierQuoteItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFQ",
                table: "RFQ",
                column: "RFQId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFQItem",
                table: "RFQItem",
                column: "RFQItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Quotes_QuoteId",
                table: "Orders",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "SupplierQuoteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteItems_ProductRefs_ProductId",
                table: "QuoteItems",
                column: "ProductId",
                principalTable: "ProductRefs",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteItems_Quotes_SupplierQuoteId",
                table: "QuoteItems",
                column: "SupplierQuoteId",
                principalTable: "Quotes",
                principalColumn: "SupplierQuoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_RFQ_RFQId",
                table: "Quotes",
                column: "RFQId",
                principalTable: "RFQ",
                principalColumn: "RFQId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Suppliers_SupplierId",
                table: "Quotes",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RFQItem_ProductRefs_ProductId",
                table: "RFQItem",
                column: "ProductId",
                principalTable: "ProductRefs",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RFQItem_RFQ_RFQId",
                table: "RFQItem",
                column: "RFQId",
                principalTable: "RFQ",
                principalColumn: "RFQId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
