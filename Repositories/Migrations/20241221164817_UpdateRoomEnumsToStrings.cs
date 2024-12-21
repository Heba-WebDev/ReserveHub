using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomEnumsToStrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Rooms",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Rooms",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Floor", "Number", "Price_Per_Night", "Status", "Type" },
                values: new object[,]
                {
                    { new Guid("023ee22e-a67f-4fc1-8801-c7437125815c"), 2, 26, 250m, "Occupied", "Suite" },
                    { new Guid("ff46c213-a342-4d06-b0d4-fe668046dbfa"), 1, 1, 175m, "Vacant", "Single" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("023ee22e-a67f-4fc1-8801-c7437125815c"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("ff46c213-a342-4d06-b0d4-fe668046dbfa"));

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Rooms",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Rooms",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
