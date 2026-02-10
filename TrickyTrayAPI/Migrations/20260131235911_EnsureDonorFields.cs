using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrickyTrayAPI.Migrations
{
    /// <inheritdoc />
    public partial class EnsureDonorFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderGift_Gifts_GiftId1",
                table: "OrderGift");

            migrationBuilder.DropIndex(
                name: "IX_OrderGift_GiftId1",
                table: "OrderGift");

            migrationBuilder.DropColumn(
                name: "GiftId1",
                table: "OrderGift");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GiftId1",
                table: "OrderGift",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderGift_GiftId1",
                table: "OrderGift",
                column: "GiftId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderGift_Gifts_GiftId1",
                table: "OrderGift",
                column: "GiftId1",
                principalTable: "Gifts",
                principalColumn: "Id");
        }
    }
}
