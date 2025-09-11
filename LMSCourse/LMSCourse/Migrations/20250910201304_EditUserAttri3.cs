using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSCourse.Migrations
{
    /// <inheritdoc />
    public partial class EditUserAttri3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreationTime", "ModificationTime", "PasswordHash", "PasswordUpdateTime" },
                values: new object[] { new DateTime(2025, 9, 10, 20, 13, 4, 505, DateTimeKind.Utc).AddTicks(8286), new DateTime(2025, 9, 10, 20, 13, 4, 505, DateTimeKind.Utc).AddTicks(8288), "AQAAAAIAAYagAAAAEFZrqt2ahj9xBfLpmbW1UFOEHnsPZqj5588xKVWKALgHTNlkgVJ2yurMAO0eMfsR7w==", new DateTime(2025, 9, 10, 20, 13, 4, 505, DateTimeKind.Utc).AddTicks(8288) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreationTime", "ModificationTime", "PasswordHash", "PasswordUpdateTime" },
                values: new object[] { new DateTime(2025, 9, 10, 20, 10, 34, 645, DateTimeKind.Utc).AddTicks(2503), new DateTime(2025, 9, 10, 20, 10, 34, 645, DateTimeKind.Utc).AddTicks(2505), "AQAAAAIAAYagAAAAEJNkebGQgZ+b5JjMz3w1GvNJbFsCtiqeZNeSLjyb46YRw85QPizilqHPDT2ENJTrSA==", new DateTime(2025, 9, 10, 20, 10, 34, 645, DateTimeKind.Utc).AddTicks(2506) });
        }
    }
}
