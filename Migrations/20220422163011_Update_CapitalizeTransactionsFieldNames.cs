using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helloAPI.Migrations
{
    public partial class Update_CapitalizeTransactionsFieldNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Products_productId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_UserDetails_buyerId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "productId",
                table: "Transactions",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "paymentId",
                table: "Transactions",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Transactions",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "buyerId",
                table: "Transactions",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_productId",
                table: "Transactions",
                newName: "IX_Transactions_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_buyerId",
                table: "Transactions",
                newName: "IX_Transactions_BuyerId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06579486-4460-413c-bc04-ea719960dff7",
                column: "ConcurrencyStamp",
                value: "f990c210-14c0-425f-82ae-771471ffd4d8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "573718cc-2ff6-4980-800d-f73e0605649a",
                column: "ConcurrencyStamp",
                value: "d3d3cad2-5249-4662-8256-a44ce5a6b4da");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb0b8efb-97e4-4302-8b84-a8670d84d0e0",
                column: "ConcurrencyStamp",
                value: "3d8a085a-aa89-4fca-9303-0cf1e1e03e69");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1464a9cd-26a7-4ca5-a42e-f8ed4e24add0", "AQAAAAEAACcQAAAAEJlIiNhcrXxTnBOPEPS3+qWVNWt5X4dfCyot1zpLGyDANnSvAgKKx2HKY+pVA6CJtg==", "62f62b6f-81e0-4ee1-82ad-d8ec2eee96d0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "46b2c7ae-290f-4b18-ae66-39db50de0379",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a2eee9d-5e32-4dc4-be86-77450b45a32f", "AQAAAAEAACcQAAAAEL5+8V0Uu8kFBbaPibhnQGx6VBqk3JN/MGEeE6fUMZ0M41foTBb4lnUP5uCnhvYDTw==", "cae0d301-9b5f-4561-a84b-9e11daf966a0" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Products_ProductId",
                table: "Transactions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_UserDetails_BuyerId",
                table: "Transactions",
                column: "BuyerId",
                principalTable: "UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Products_ProductId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_UserDetails_BuyerId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Transactions",
                newName: "productId");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Transactions",
                newName: "paymentId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Transactions",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Transactions",
                newName: "buyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ProductId",
                table: "Transactions",
                newName: "IX_Transactions_productId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_BuyerId",
                table: "Transactions",
                newName: "IX_Transactions_buyerId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06579486-4460-413c-bc04-ea719960dff7",
                column: "ConcurrencyStamp",
                value: "a462ff4c-10d6-485c-9aa4-c6d3363104b9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "573718cc-2ff6-4980-800d-f73e0605649a",
                column: "ConcurrencyStamp",
                value: "c883e5ee-ba96-4547-8fc3-fb1c8eccb7fc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb0b8efb-97e4-4302-8b84-a8670d84d0e0",
                column: "ConcurrencyStamp",
                value: "38e688d6-7720-4f50-86b1-a5e6291d2f1a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f3a1459-d9b4-4d54-b9aa-e9ebf0512761", "AQAAAAEAACcQAAAAEPylTktY8pV5rGpwGAHx1KxKgL8tnebVr5lI5mKEmu30yJy4xaGgRM3Pifw3W/5gzQ==", "b51a0423-b7dc-485b-8204-6667545fbc18" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "46b2c7ae-290f-4b18-ae66-39db50de0379",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8c2ba56e-cec0-4521-81e0-003820f65ad0", "AQAAAAEAACcQAAAAEA8st63Fz1fgs4XVQDWHn8HDWxxuey0o4HGnbPSOdZGqoanncp2fzcbjPcm43VYUiA==", "f14981c8-7bc9-478d-bc7f-457b05ee8e60" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Products_productId",
                table: "Transactions",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_UserDetails_buyerId",
                table: "Transactions",
                column: "buyerId",
                principalTable: "UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
