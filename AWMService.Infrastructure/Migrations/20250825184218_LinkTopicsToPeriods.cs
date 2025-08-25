using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AWMService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkTopicsToPeriods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeriodId",
                table: "Topics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_PeriodId",
                table: "Topics",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Periods_PeriodId",
                table: "Topics",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Periods_PeriodId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_PeriodId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "Topics");
        }
    }
}
