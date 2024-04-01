using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarciaApi.Migrations
{
    /// <inheritdoc />
    public partial class tenth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemProduct_Items_AdditionalItemsItemId",
                table: "ItemProduct");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "AdditionalItemsItemId",
                table: "ItemProduct",
                newName: "ItemsItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemProduct_Items_ItemsItemId",
                table: "ItemProduct",
                column: "ItemsItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemProduct_Items_ItemsItemId",
                table: "ItemProduct");

            migrationBuilder.RenameColumn(
                name: "ItemsItemId",
                table: "ItemProduct",
                newName: "AdditionalItemsItemId");

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Items",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemProduct_Items_AdditionalItemsItemId",
                table: "ItemProduct",
                column: "AdditionalItemsItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
