using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helloAPI.Migrations
{
    public partial class Add_Status_Products_and_Transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06579486-4460-413c-bc04-ea719960dff7",
                column: "ConcurrencyStamp",
                value: "d5c29371-3ff0-4623-b93c-949f1883ed37");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "573718cc-2ff6-4980-800d-f73e0605649a",
                column: "ConcurrencyStamp",
                value: "4fb225ed-8d4b-48fb-be85-dbaa31b6a522");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb0b8efb-97e4-4302-8b84-a8670d84d0e0",
                column: "ConcurrencyStamp",
                value: "6eaedf65-bcc2-4752-8597-e16e34349d57");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "42626ca1-5d96-489b-8deb-7d9ed4232ed7", "AQAAAAEAACcQAAAAEDB3EyuwJyshgO/fSjXcFCkh5nsVPehiPxRYqr2m82aEPRMjuX/FMI4a/sULsyvkag==", "146bcdda-0c0b-48bb-8cc0-c6abc4f8c35a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "46b2c7ae-290f-4b18-ae66-39db50de0379",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bfd0c76f-a181-4840-84da-cd1fe8c36445", "AQAAAAEAACcQAAAAEIjGojvK84yYhHAGuwP+/Zok67LLvlZz7/3O6vM8kCjidemNIq0wajKcZsel2i8SDQ==", "028e076c-5129-43e5-beb7-ebf489666493" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");

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
        }
    }
}
