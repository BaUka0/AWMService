using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AWMService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkDirectionsToPeriods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeriodId",
                table: "Directions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AcademicYears",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AcademicYears",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Directions_PeriodId",
                table: "Directions",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Directions_Periods_PeriodId",
                table: "Directions",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directions_Periods_PeriodId",
                table: "Directions");

            migrationBuilder.DropIndex(
                name: "IX_Directions_PeriodId",
                table: "Directions");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "Directions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AcademicYears",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AcademicYears",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
