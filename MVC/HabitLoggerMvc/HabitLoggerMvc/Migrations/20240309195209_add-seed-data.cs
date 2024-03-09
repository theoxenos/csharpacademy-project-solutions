using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitLoggerMvc.Migrations
{
    /// <inheritdoc />
    public partial class addseeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "HabitUnits",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Medium glass" },
                    { 2, "Small glass" },
                    { 3, "Meters" },
                    { 4, "Minutes" },
                    { 5, "Pages" }
                });

            migrationBuilder.InsertData(
                table: "Habits",
                columns: new[] { "Id", "HabitUnitId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Drinking water" },
                    { 2, 2, "Drinking fruit sap" },
                    { 3, 3, "Walking" },
                    { 4, 4, "Meditation" },
                    { 5, 5, "Reading" }
                });

            migrationBuilder.InsertData(
                table: "HabitLogs",
                columns: new[] { "Id", "Date", "HabitId", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateOnly(2023, 1, 1), 1, 8 },
                    { 2, new DateOnly(2023, 1, 2), 2, 5 },
                    { 3, new DateOnly(2023, 1, 3), 3, 3 },
                    { 4, new DateOnly(2023, 1, 4), 1, 7 },
                    { 5, new DateOnly(2023, 1, 5), 2, 4 },
                    { 6, new DateOnly(2023, 1, 6), 4, 30 },
                    { 7, new DateOnly(2023, 1, 7), 5, 150 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HabitLogs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HabitLogs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "HabitLogs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "HabitLogs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "HabitLogs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "HabitLogs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "HabitLogs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "HabitUnits",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HabitUnits",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "HabitUnits",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "HabitUnits",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "HabitUnits",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
