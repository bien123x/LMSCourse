using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSCourse.Migrations
{
    /// <inheritdoc />
    public partial class EditUserAttri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "FailedAccessCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEndTime",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationTime",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordUpdateTime",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "CreationTime", "LockoutEndTime", "ModificationTime", "ModifiedBy", "PasswordHash", "PasswordUpdateTime" },
                values: new object[] { "", new DateTime(2025, 9, 10, 20, 9, 6, 29, DateTimeKind.Utc).AddTicks(8602), null, new DateTime(2025, 9, 10, 20, 9, 6, 29, DateTimeKind.Utc).AddTicks(8604), "", "AQAAAAIAAYagAAAAEB4GjqoaACirUbTecrx2O8jheQKTstyMw0NjGOmZHRGTn/Jud41tycddYep6PiKI2A==", new DateTime(2025, 9, 10, 20, 9, 6, 29, DateTimeKind.Utc).AddTicks(8604) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FailedAccessCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEndTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModificationTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordUpdateTime",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMzknX2MYGHJeLe0/W3+YGSoeIkEBeYukkowMdWyLZYK6ylmGx1KcNXX9iYYdgWcqw==");
        }
    }
}
