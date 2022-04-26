using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helloAPI.Migrations
{
    public partial class change_transactionspaymentid_tostring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentId",
                table: "Transactions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06579486-4460-413c-bc04-ea719960dff7",
                column: "ConcurrencyStamp",
                value: "b24fea27-0d63-4e73-966d-2e85a95b382d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "573718cc-2ff6-4980-800d-f73e0605649a",
                column: "ConcurrencyStamp",
                value: "7e0158f4-ca4e-4b4b-9ce8-5aeea27b4c2f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb0b8efb-97e4-4302-8b84-a8670d84d0e0",
                column: "ConcurrencyStamp",
                value: "b8a6fcc5-b2f7-454c-ab6d-9b09799341ad");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02df80ef-5e06-4a07-81c4-d1524d7ad8c7", "AQAAAAEAACcQAAAAEPO9NPfk7Kkajsd0/70pWWNbJ5kmeCQqSVJRvPgJWZXGnAxUITT0xTG9s5yw9LEKSA==", "c9d469c6-ebcb-499a-a0ad-e780d1633bc3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "46b2c7ae-290f-4b18-ae66-39db50de0379",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26e9b515-f502-4294-bd3d-e11e9394bc02", "AQAAAAEAACcQAAAAEJd45m3m47H0bIpw1s02cqFn7bi1QQNiEwlwosnWR8PE1TWouZMUHarbawle9XWaYg==", "b305bc43-a96a-439a-a218-ca6ffd63918e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06579486-4460-413c-bc04-ea719960dff7",
                column: "ConcurrencyStamp",
                value: "49a1b9a2-71f3-449c-a7b1-5d2bdf2d68ce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "573718cc-2ff6-4980-800d-f73e0605649a",
                column: "ConcurrencyStamp",
                value: "a0ac4852-8835-40b6-8d92-03e864d4355e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb0b8efb-97e4-4302-8b84-a8670d84d0e0",
                column: "ConcurrencyStamp",
                value: "ab3565e9-6446-4121-a497-45d041f634bc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6d35a66b-b076-443b-aca4-9cf371cf64a1", "AQAAAAEAACcQAAAAENFpL8CzMa83Zoxf8dT/tz5srOySMPRzvcA8yHOQksELJKNeUWJfjSmgs32Y9JK1Rw==", "21bc61b3-5226-4c39-8cbe-46047ff5ddb4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "46b2c7ae-290f-4b18-ae66-39db50de0379",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a641bf6b-ebdd-4ec1-8886-164a469c9ef1", "AQAAAAEAACcQAAAAEHZvGXFEcTvLg4x9dOJoqNnaYFhPSV6EG9fNfITTBm5SXy3tyZYO59gAmRHYYCmONw==", "deb395d1-335e-48ca-b09c-83217089c2c5" });
        }
    }
}
