using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helloAPI.Migrations
{
    public partial class REDO__added_transactions_table_new_datefield_on_Products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "Products",
                newName: "Date");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    buyerId = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    paymentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_UserDetails_buyerId",
                        column: x => x.buyerId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_buyerId",
                table: "Transactions",
                column: "buyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_productId",
                table: "Transactions",
                column: "productId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Products",
                newName: "date");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06579486-4460-413c-bc04-ea719960dff7",
                column: "ConcurrencyStamp",
                value: "ecbd75f3-42f4-456c-a663-c1aed7a26aff");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "573718cc-2ff6-4980-800d-f73e0605649a",
                column: "ConcurrencyStamp",
                value: "e6ca0b10-afc6-4b20-a0f4-fa69be29f6b8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb0b8efb-97e4-4302-8b84-a8670d84d0e0",
                column: "ConcurrencyStamp",
                value: "07b00cd9-c266-4a37-92ab-95d55b9323c6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e5253083-e4a2-432a-8fc3-43eb225fb5e2", "AQAAAAEAACcQAAAAEOAwlfevaF+vCsfxTt4T/Xj6oZNVY/FBoZsqFK48OoWCTnv115mwH7D331P8IFTf4Q==", "72d0e27c-3fea-4133-aa8a-e87c2d6af91e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "46b2c7ae-290f-4b18-ae66-39db50de0379",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "40e0ab63-8c35-4dad-9e1d-4ac1fecb61d7", "AQAAAAEAACcQAAAAEKDgLXRw1bzJthiZEgHUuwfrKjprlh9M8gL5uuabXD0fbQuvs2WAYLsQvSrvZl3vKg==", "639b301a-4e05-4e8a-9ef3-f0192ca6bbc3" });
        }
    }
}
