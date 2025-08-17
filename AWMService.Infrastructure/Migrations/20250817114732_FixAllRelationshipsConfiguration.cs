using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AWMService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAllRelationshipsConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_CommissionTypes_CommissionTypesId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Institutes_InstitutesId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Periods_AcademicYears_AcademicYearsId",
                table: "Periods");

            migrationBuilder.DropForeignKey(
                name: "FK_Periods_PeriodTypes_PeriodTypesId",
                table: "Periods");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RolesId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentWork_Topics_TopicsId",
                table: "StudentWork");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentWork_WorkTypes_WorkTypesId",
                table: "StudentWork");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Directions_DirectionsId",
                table: "Topics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentsId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserTypes_UserTypesId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartmentsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserTypesId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RolesId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Topics_DirectionsId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_StudentWork_TopicsId",
                table: "StudentWork");

            migrationBuilder.DropIndex(
                name: "IX_StudentWork_WorkTypesId",
                table: "StudentWork");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RolesId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_Periods_AcademicYearsId",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Periods_PeriodTypesId",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Departments_InstitutesId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Commissions_CommissionTypesId",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "DepartmentsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserTypesId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RolesId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DirectionsId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "TopicsId",
                table: "StudentWork");

            migrationBuilder.DropColumn(
                name: "WorkTypesId",
                table: "StudentWork");

            migrationBuilder.DropColumn(
                name: "RolesId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "AcademicYearsId",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "PeriodTypesId",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "InstitutesId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CommissionTypesId",
                table: "Commissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentsId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserTypesId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RolesId",
                table: "UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DirectionsId",
                table: "Topics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TopicsId",
                table: "StudentWork",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkTypesId",
                table: "StudentWork",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RolesId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicYearsId",
                table: "Periods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodTypesId",
                table: "Periods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutesId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommissionTypesId",
                table: "Commissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentsId",
                table: "Users",
                column: "DepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypesId",
                table: "Users",
                column: "UserTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RolesId",
                table: "UserRoles",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_DirectionsId",
                table: "Topics",
                column: "DirectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_TopicsId",
                table: "StudentWork",
                column: "TopicsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_WorkTypesId",
                table: "StudentWork",
                column: "WorkTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RolesId",
                table: "RolePermissions",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_AcademicYearsId",
                table: "Periods",
                column: "AcademicYearsId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_PeriodTypesId",
                table: "Periods",
                column: "PeriodTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_InstitutesId",
                table: "Departments",
                column: "InstitutesId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_CommissionTypesId",
                table: "Commissions",
                column: "CommissionTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_CommissionTypes_CommissionTypesId",
                table: "Commissions",
                column: "CommissionTypesId",
                principalTable: "CommissionTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Institutes_InstitutesId",
                table: "Departments",
                column: "InstitutesId",
                principalTable: "Institutes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_AcademicYears_AcademicYearsId",
                table: "Periods",
                column: "AcademicYearsId",
                principalTable: "AcademicYears",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_PeriodTypes_PeriodTypesId",
                table: "Periods",
                column: "PeriodTypesId",
                principalTable: "PeriodTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RolesId",
                table: "RolePermissions",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentWork_Topics_TopicsId",
                table: "StudentWork",
                column: "TopicsId",
                principalTable: "Topics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentWork_WorkTypes_WorkTypesId",
                table: "StudentWork",
                column: "WorkTypesId",
                principalTable: "WorkTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Directions_DirectionsId",
                table: "Topics",
                column: "DirectionsId",
                principalTable: "Directions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentsId",
                table: "Users",
                column: "DepartmentsId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserTypes_UserTypesId",
                table: "Users",
                column: "UserTypesId",
                principalTable: "UserTypes",
                principalColumn: "Id");
        }
    }
}
