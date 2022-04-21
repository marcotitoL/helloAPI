using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helloAPI.Migrations
{
    public partial class added_transactions_table_new_datefield_on_Products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "Products",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date",
                table: "Products");

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "46b2c7ae-290f-4b18-ae66-39db50de0379",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "989b0ddf-80f1-48c5-8b2a-ad8f54a6f270", "AQAAAAEAACcQAAAAEIS4yMuGohHPXtog8vDJ+L6tG8jhCouTaQPHR6k5z+0BO0zVoTjmeZW8ZHrGDNtPzA==", "7eee85db-55bc-41d6-9c1e-4de5a5bc231d" });
        }
    }
}
