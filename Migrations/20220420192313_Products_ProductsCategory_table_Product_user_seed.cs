using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helloAPI.Migrations
{
    public partial class Products_ProductsCategory_table_Product_user_seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductsCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCategory", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductsCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductsCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_UserDetails_UserId",
                        column: x => x.UserId,
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
                value: "d6bde420-00d5-46b1-95e2-c8b0c9270cf2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "573718cc-2ff6-4980-800d-f73e0605649a",
                column: "ConcurrencyStamp",
                value: "adbf697f-dfad-4575-87a3-c4691f0da525");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb0b8efb-97e4-4302-8b84-a8670d84d0e0",
                column: "ConcurrencyStamp",
                value: "9eb657bc-44a6-4a45-810b-4dd1e541a81f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "200d86a4-3cd1-4964-b886-5a2be2fbed53", "AQAAAAEAACcQAAAAEBc3W2kOkXigR9SI8hrFtSGwSDJmsJxEugYUOK3ZQOJuyq0Kj/nqVDVK/x8EmKhYzg==", "d9e1579b-f08e-46d8-b90a-f0c3ab525528" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "46b2c7ae-290f-4b18-ae66-39db50de0379", 0, "989b0ddf-80f1-48c5-8b2a-ad8f54a6f270", "myemail@somedomain.com", true, false, null, "MYEMAIL@SOMEDOMAIN.COM", "MYEMAIL@SOMEDOMAIN.COM", "AQAAAAEAACcQAAAAEIS4yMuGohHPXtog8vDJ+L6tG8jhCouTaQPHR6k5z+0BO0zVoTjmeZW8ZHrGDNtPzA==", null, true, "7eee85db-55bc-41d6-9c1e-4de5a5bc231d", false, "myemail@somedomain.com" });

            migrationBuilder.InsertData(
                table: "ProductsCategory",
                columns: new[] { "Id", "CategoryName", "Guid" },
                values: new object[,]
                {
                    { 1, "Miscellaneous", "e2cfd51c-47cc-4b86-b0de-216ede287fe5" },
                    { 2, "Hats", "9005481f-5f2a-470f-9357-3115f37b9f81" }
                });

            migrationBuilder.UpdateData(
                table: "UserDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "Balance",
                value: 1000.0m);

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "06579486-4460-413c-bc04-ea719960dff7", "46b2c7ae-290f-4b18-ae66-39db50de0379" });

            migrationBuilder.InsertData(
                table: "UserDetails",
                columns: new[] { "Id", "AspNetUserId", "Balance", "Birthdate", "Firstname", "Lastname", "ProfileImage" },
                values: new object[] { 99, "46b2c7ae-290f-4b18-ae66-39db50de0379", 4.30m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", "Doe", null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Guid", "Name", "Price", "Qty", "UserId" },
                values: new object[] { 1, 2, "slightly used running cap, still works great", "b7b2522b-df88-4286-a608-c9a7587d1b7a", "Adidas Running Cap", 29.50m, 1, 99 });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserId",
                table: "Products",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductsCategory");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "06579486-4460-413c-bc04-ea719960dff7", "46b2c7ae-290f-4b18-ae66-39db50de0379" });

            migrationBuilder.DeleteData(
                table: "UserDetails",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "46b2c7ae-290f-4b18-ae66-39db50de0379");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06579486-4460-413c-bc04-ea719960dff7",
                column: "ConcurrencyStamp",
                value: "49b9eac1-a24b-41b9-b7fd-663e4022af88");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "573718cc-2ff6-4980-800d-f73e0605649a",
                column: "ConcurrencyStamp",
                value: "5ce40985-bedc-49f4-b3be-1d70ec5d53d3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb0b8efb-97e4-4302-8b84-a8670d84d0e0",
                column: "ConcurrencyStamp",
                value: "7db30c28-fdff-419b-abd5-6f53a3e19ebd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "224f0378-c51b-4827-b83d-24893b50233c", "AQAAAAEAACcQAAAAEOYdrPYBWShhPPcwSoZeLJVLRrF8vD/h0OMgH0ZtBeUuKLgdfT8SpMBzOUlugT4pSQ==", "10d1df60-88e8-4181-8e26-147b609a0658" });

            migrationBuilder.UpdateData(
                table: "UserDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "Balance",
                value: 0.0m);
        }
    }
}
