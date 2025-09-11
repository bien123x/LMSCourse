using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSCourse.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPermission2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMzknX2MYGHJeLe0/W3+YGSoeIkEBeYukkowMdWyLZYK6ylmGx1KcNXX9iYYdgWcqw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN0wsVQh/z+Uyt3hiEINeCfSDBXSA7BUYsCOqTYuPL6kziHuXl+Dst1u6AphtZxndw==");
        }
    }
}
