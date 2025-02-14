using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class CustomerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7cb22b1-8e9a-4bf5-876f-a9aefca3a28f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb5bbe89-ea83-4d8d-83bd-7322eea69d1e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1233289-6053-41ca-b635-376248f99c06");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3ccbad3d-74e7-4c80-841f-a6ffd8121069", null, "Admin", "ADMIN" },
                    { "a96f03fb-71a1-4c7c-a9f6-0f201162da8f", null, "Staff", "STAFF" },
                    { "feb93cb1-d6e3-4492-b87c-87582f350d05", null, "Customer", "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ccbad3d-74e7-4c80-841f-a6ffd8121069");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a96f03fb-71a1-4c7c-a9f6-0f201162da8f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "feb93cb1-d6e3-4492-b87c-87582f350d05");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b7cb22b1-8e9a-4bf5-876f-a9aefca3a28f", null, "Customer", "Customer" },
                    { "cb5bbe89-ea83-4d8d-83bd-7322eea69d1e", null, "Admin", "ADMIN" },
                    { "e1233289-6053-41ca-b635-376248f99c06", null, "User", "USER" }
                });
        }
    }
}
