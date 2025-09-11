using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSCourse.Migrations
{
    /// <inheritdoc />
    public partial class EditUserAttri2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreationTime", "ModificationTime", "PasswordHash", "PasswordUpdateTime" },
                values: new object[] { new DateTime(2025, 9, 10, 20, 10, 34, 645, DateTimeKind.Utc).AddTicks(2503), new DateTime(2025, 9, 10, 20, 10, 34, 645, DateTimeKind.Utc).AddTicks(2505), "AQAAAAIAAYagAAAAEJNkebGQgZ+b5JjMz3w1GvNJbFsCtiqeZNeSLjyb46YRw85QPizilqHPDT2ENJTrSA==", new DateTime(2025, 9, 10, 20, 10, 34, 645, DateTimeKind.Utc).AddTicks(2506) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreationTime", "ModificationTime", "PasswordHash", "PasswordUpdateTime" },
                values: new object[] { new DateTime(2025, 9, 10, 20, 9, 6, 29, DateTimeKind.Utc).AddTicks(8602), new DateTime(2025, 9, 10, 20, 9, 6, 29, DateTimeKind.Utc).AddTicks(8604), "AQAAAAIAAYagAAAAEB4GjqoaACirUbTecrx2O8jheQKTstyMw0NjGOmZHRGTn/Jud41tycddYep6PiKI2A==", new DateTime(2025, 9, 10, 20, 9, 6, 29, DateTimeKind.Utc).AddTicks(8604) });
        }
    }
}
