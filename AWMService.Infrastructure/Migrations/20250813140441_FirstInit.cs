using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AWMService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Institutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    InstituteId = table.Column<int>(type: "int", nullable: false),
                    InstitutesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Institutes_InstituteId",
                        column: x => x.InstituteId,
                        principalTable: "Institutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_Institutes_InstitutesId",
                        column: x => x.InstitutesId,
                        principalTable: "Institutes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SurName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IIN = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DepartmentsId = table.Column<int>(type: "int", nullable: true),
                    UserTypesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_UserTypes_UserTypesId",
                        column: x => x.UserTypesId,
                        principalTable: "UserTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AcademicYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicYears_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicYears_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicYears_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckTypes_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommissionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommissionTypes_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Organization = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalContacts_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalContacts_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalContacts_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReadOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeriodTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeriodTypes_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingKey);
                    table.ForeignKey(
                        name: "FK_Settings_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Settings_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statuses_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    AssignedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedBy = table.Column<int>(type: "int", nullable: true),
                    RolesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_RevokedBy",
                        column: x => x.RevokedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTypes_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupervisorApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedBy = table.Column<int>(type: "int", nullable: true),
                    RevokedByUserId = table.Column<int>(type: "int", nullable: true),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupervisorApprovals_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupervisorApprovals_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupervisorApprovals_Users_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupervisorApprovals_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupervisorApprovals_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupervisorApprovals_Users_RevokedBy",
                        column: x => x.RevokedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupervisorApprovals_Users_RevokedByUserId",
                        column: x => x.RevokedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupervisorApprovals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentExperts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CheckTypeId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    AssignedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedBy = table.Column<int>(type: "int", nullable: true),
                    RevokedByUserId = table.Column<int>(type: "int", nullable: true),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentExperts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentExperts_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentExperts_CheckTypes_CheckTypeId",
                        column: x => x.CheckTypeId,
                        principalTable: "CheckTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentExperts_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentExperts_Users_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentExperts_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentExperts_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentExperts_Users_RevokedBy",
                        column: x => x.RevokedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentExperts_Users_RevokedByUserId",
                        column: x => x.RevokedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentExperts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationCriteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CommissionTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationCriteria_CommissionTypes_CommissionTypeId",
                        column: x => x.CommissionTypeId,
                        principalTable: "CommissionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationCriteria_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    AssignedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedBy = table.Column<int>(type: "int", nullable: true),
                    RolesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolePermissions_Users_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Users_RevokedBy",
                        column: x => x.RevokedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Directions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameKz = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NameRu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Directions_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Directions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Directions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Directions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Directions_Users_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodTypeId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearsId = table.Column<int>(type: "int", nullable: true),
                    PeriodTypesId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                    table.CheckConstraint("CK_Periods_Dates", "[StartDate] <= [EndDate]");
                    table.ForeignKey(
                        name: "FK_Periods_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Periods_AcademicYears_AcademicYearsId",
                        column: x => x.AcademicYearsId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Periods_PeriodTypes_PeriodTypeId",
                        column: x => x.PeriodTypeId,
                        principalTable: "PeriodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Periods_PeriodTypes_PeriodTypesId",
                        column: x => x.PeriodTypesId,
                        principalTable: "PeriodTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Periods_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Periods_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Periods_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Periods_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectionId = table.Column<int>(type: "int", nullable: false),
                    TitleKz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TitleRu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    SupervisorId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    DirectionsId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Topics_Directions_DirectionsId",
                        column: x => x.DirectionsId,
                        principalTable: "Directions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Topics_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topics_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topics_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topics_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topics_Users_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CommissionTypeId = table.Column<int>(type: "int", nullable: false),
                    SecretaryId = table.Column<int>(type: "int", nullable: false),
                    PeriodId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CommissionTypesId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commissions_CommissionTypes_CommissionTypeId",
                        column: x => x.CommissionTypeId,
                        principalTable: "CommissionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commissions_CommissionTypes_CommissionTypesId",
                        column: x => x.CommissionTypesId,
                        principalTable: "CommissionTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Commissions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commissions_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commissions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commissions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commissions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commissions_Users_SecretaryId",
                        column: x => x.SecretaryId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentWork",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    WorkTypeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    FinalGrade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkTypesId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentWork", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentWork_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentWork_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentWork_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentWork_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentWork_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentWork_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentWork_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentWork_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentWork_WorkTypes_WorkTypesId",
                        column: x => x.WorkTypesId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommissionMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommissionId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    ExternalContactId = table.Column<int>(type: "int", nullable: true),
                    RoleInCommission = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssignedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    RemovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionMembers", x => x.Id);
                    table.CheckConstraint("CK_CommissionMembers_EitherMemberOrExternal", "([MemberId] IS NOT NULL) OR ([ExternalContactId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_CommissionMembers_Commissions_CommissionId",
                        column: x => x.CommissionId,
                        principalTable: "Commissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommissionMembers_ExternalContacts_ExternalContactId",
                        column: x => x.ExternalContactId,
                        principalTable: "ExternalContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommissionMembers_Users_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommissionMembers_Users_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommissionMembers_Users_RemovedBy",
                        column: x => x.RemovedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DefenseSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommissionId = table.Column<int>(type: "int", nullable: false),
                    StudentWorkId = table.Column<int>(type: "int", nullable: false),
                    DefenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefenseSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefenseSchedules_Commissions_CommissionId",
                        column: x => x.CommissionId,
                        principalTable: "Commissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefenseSchedules_StudentWork_StudentWorkId",
                        column: x => x.StudentWorkId,
                        principalTable: "StudentWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefenseSchedules_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefenseSchedules_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefenseSchedules_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkChecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentWorkId = table.Column<int>(type: "int", nullable: false),
                    CheckTypeId = table.Column<int>(type: "int", nullable: false),
                    ExpertId = table.Column<int>(type: "int", nullable: true),
                    ReviewerId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkChecks", x => x.Id);
                    table.CheckConstraint("CK_WorkChecks_ExpertOrReviewer", "([ExpertId] IS NOT NULL) OR ([ReviewerId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_WorkChecks_CheckTypes_CheckTypeId",
                        column: x => x.CheckTypeId,
                        principalTable: "CheckTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkChecks_ExternalContacts_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "ExternalContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkChecks_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkChecks_StudentWork_StudentWorkId",
                        column: x => x.StudentWorkId,
                        principalTable: "StudentWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkChecks_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkChecks_Users_ExpertId",
                        column: x => x.ExpertId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkChecks_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DefenseGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefenseScheduleId = table.Column<int>(type: "int", nullable: false),
                    FinalScore = table.Column<double>(type: "float", nullable: false),
                    FinalGrade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefenseGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefenseGrades_DefenseSchedules_DefenseScheduleId",
                        column: x => x.DefenseScheduleId,
                        principalTable: "DefenseSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefenseGrades_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefenseGrades_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefenseGrades_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentWorkId = table.Column<int>(type: "int", nullable: true),
                    WorkCheckId = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedBy = table.Column<int>(type: "int", nullable: false),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.CheckConstraint("CK_Attachments_FileSize_NonNegative", "[FileSize] >= 0");
                    table.ForeignKey(
                        name: "FK_Attachments_StudentWork_StudentWorkId",
                        column: x => x.StudentWorkId,
                        principalTable: "StudentWork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachments_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attachments_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attachments_Users_UploadedBy",
                        column: x => x.UploadedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attachments_WorkChecks_WorkCheckId",
                        column: x => x.WorkCheckId,
                        principalTable: "WorkChecks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EvaluationScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefenseGradeId = table.Column<int>(type: "int", nullable: false),
                    CriteriaId = table.Column<int>(type: "int", nullable: false),
                    CommissionMemberId = table.Column<int>(type: "int", nullable: false),
                    ScoreValue = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationScores", x => x.Id);
                    table.CheckConstraint("CK_EvaluationScores_ScoreValue_NonNegative", "[ScoreValue] >= 0");
                    table.ForeignKey(
                        name: "FK_EvaluationScores_CommissionMembers_CommissionMemberId",
                        column: x => x.CommissionMemberId,
                        principalTable: "CommissionMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationScores_DefenseGrades_DefenseGradeId",
                        column: x => x.DefenseGradeId,
                        principalTable: "DefenseGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluationScores_EvaluationCriteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "EvaluationCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationScores_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationScores_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_CreatedBy",
                table: "AcademicYears",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_DeletedBy",
                table: "AcademicYears",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_ModifiedBy",
                table: "AcademicYears",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_YearName",
                table: "AcademicYears",
                column: "YearName");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CreatedBy",
                table: "Applications",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CreatedOn",
                table: "Applications",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ModifiedBy",
                table: "Applications",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_StatusId",
                table: "Applications",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_StudentId_TopicId",
                table: "Applications",
                columns: new[] { "StudentId", "TopicId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TopicId",
                table: "Applications",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_DeletedBy",
                table: "Attachments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ModifiedBy",
                table: "Attachments",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_StudentWorkId",
                table: "Attachments",
                column: "StudentWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UploadedBy",
                table: "Attachments",
                column: "UploadedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UploadedOn",
                table: "Attachments",
                column: "UploadedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_WorkCheckId",
                table: "Attachments",
                column: "WorkCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckTypes_DeletedBy",
                table: "CheckTypes",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckTypes_Name",
                table: "CheckTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionMembers_AssignedBy",
                table: "CommissionMembers",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionMembers_CommissionId_ExternalContactId",
                table: "CommissionMembers",
                columns: new[] { "CommissionId", "ExternalContactId" },
                unique: true,
                filter: "[ExternalContactId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionMembers_CommissionId_MemberId",
                table: "CommissionMembers",
                columns: new[] { "CommissionId", "MemberId" },
                unique: true,
                filter: "[MemberId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionMembers_ExternalContactId",
                table: "CommissionMembers",
                column: "ExternalContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionMembers_MemberId",
                table: "CommissionMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionMembers_RemovedBy",
                table: "CommissionMembers",
                column: "RemovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_CommissionTypeId",
                table: "Commissions",
                column: "CommissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_CommissionTypesId",
                table: "Commissions",
                column: "CommissionTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_CreatedBy",
                table: "Commissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_DeletedBy",
                table: "Commissions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_DepartmentId_PeriodId_CommissionTypeId",
                table: "Commissions",
                columns: new[] { "DepartmentId", "PeriodId", "CommissionTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_ModifiedBy",
                table: "Commissions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_PeriodId",
                table: "Commissions",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_SecretaryId",
                table: "Commissions",
                column: "SecretaryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionTypes_DeletedBy",
                table: "CommissionTypes",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionTypes_Name",
                table: "CommissionTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseGrades_CreatedBy",
                table: "DefenseGrades",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseGrades_DefenseScheduleId",
                table: "DefenseGrades",
                column: "DefenseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseGrades_ModifiedBy",
                table: "DefenseGrades",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseGrades_StatusId",
                table: "DefenseGrades",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseSchedules_CommissionId",
                table: "DefenseSchedules",
                column: "CommissionId");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseSchedules_CreatedBy",
                table: "DefenseSchedules",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseSchedules_DefenseDate",
                table: "DefenseSchedules",
                column: "DefenseDate");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseSchedules_DeletedBy",
                table: "DefenseSchedules",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseSchedules_ModifiedBy",
                table: "DefenseSchedules",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseSchedules_StudentWorkId",
                table: "DefenseSchedules",
                column: "StudentWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExperts_AcademicYearId",
                table: "DepartmentExperts",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExperts_AssignedBy",
                table: "DepartmentExperts",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExperts_CheckTypeId",
                table: "DepartmentExperts",
                column: "CheckTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExperts_DepartmentId_CheckTypeId_AcademicYearId_UserId",
                table: "DepartmentExperts",
                columns: new[] { "DepartmentId", "CheckTypeId", "AcademicYearId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExperts_ModifiedBy",
                table: "DepartmentExperts",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExperts_ModifiedByUserId",
                table: "DepartmentExperts",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExperts_RevokedBy",
                table: "DepartmentExperts",
                column: "RevokedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExperts_RevokedByUserId",
                table: "DepartmentExperts",
                column: "RevokedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExperts_UserId",
                table: "DepartmentExperts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_InstituteId",
                table: "Departments",
                column: "InstituteId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_InstituteId_Name",
                table: "Departments",
                columns: new[] { "InstituteId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_InstitutesId",
                table: "Departments",
                column: "InstitutesId");

            migrationBuilder.CreateIndex(
                name: "IX_Directions_CreatedBy",
                table: "Directions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Directions_DeletedBy",
                table: "Directions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Directions_ModifiedBy",
                table: "Directions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Directions_StatusId",
                table: "Directions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Directions_SupervisorId",
                table: "Directions",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationCriteria_CommissionTypeId_Name",
                table: "EvaluationCriteria",
                columns: new[] { "CommissionTypeId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationCriteria_DeletedBy",
                table: "EvaluationCriteria",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationScores_CommissionMemberId",
                table: "EvaluationScores",
                column: "CommissionMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationScores_CreatedBy",
                table: "EvaluationScores",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationScores_CriteriaId",
                table: "EvaluationScores",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationScores_DefenseGradeId",
                table: "EvaluationScores",
                column: "DefenseGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationScores_ModifiedBy",
                table: "EvaluationScores",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalContacts_CreatedBy",
                table: "ExternalContacts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalContacts_DeletedBy",
                table: "ExternalContacts",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalContacts_FullName",
                table: "ExternalContacts",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalContacts_ModifiedBy",
                table: "ExternalContacts",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalContacts_Organization_FullName",
                table: "ExternalContacts",
                columns: new[] { "Organization", "FullName" });

            migrationBuilder.CreateIndex(
                name: "IX_Institutes_Name",
                table: "Institutes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedOn",
                table: "Notifications",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead_CreatedOn",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Periods_AcademicYearId_PeriodTypeId",
                table: "Periods",
                columns: new[] { "AcademicYearId", "PeriodTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Periods_AcademicYearsId",
                table: "Periods",
                column: "AcademicYearsId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_CreatedBy",
                table: "Periods",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_DeletedBy",
                table: "Periods",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_EndDate",
                table: "Periods",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_ModifiedBy",
                table: "Periods",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_PeriodTypeId",
                table: "Periods",
                column: "PeriodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_PeriodTypesId",
                table: "Periods",
                column: "PeriodTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_StartDate",
                table: "Periods",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_StatusId",
                table: "Periods",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodTypes_DeletedBy",
                table: "PeriodTypes",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodTypes_Name",
                table: "PeriodTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CreatedBy",
                table: "Permissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_DeletedBy",
                table: "Permissions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ModifiedBy",
                table: "Permissions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_AssignedBy",
                table: "RolePermissions",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RevokedBy",
                table: "RolePermissions",
                column: "RevokedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RolesId",
                table: "RolePermissions",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_CreatedBy",
                table: "Settings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_ModifiedBy",
                table: "Settings",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_SettingKey",
                table: "Settings",
                column: "SettingKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_DeletedBy",
                table: "Statuses",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Name",
                table: "Statuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_AcademicYearId",
                table: "StudentWork",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_CreatedBy",
                table: "StudentWork",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_DeletedBy",
                table: "StudentWork",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_ModifiedBy",
                table: "StudentWork",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_StatusId",
                table: "StudentWork",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_StudentId",
                table: "StudentWork",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_StudentId_AcademicYearId_WorkTypeId",
                table: "StudentWork",
                columns: new[] { "StudentId", "AcademicYearId", "WorkTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_TopicId",
                table: "StudentWork",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_WorkTypeId",
                table: "StudentWork",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWork_WorkTypesId",
                table: "StudentWork",
                column: "WorkTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorApprovals_AcademicYearId",
                table: "SupervisorApprovals",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorApprovals_ApprovedBy",
                table: "SupervisorApprovals",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorApprovals_DepartmentId",
                table: "SupervisorApprovals",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorApprovals_ModifiedBy",
                table: "SupervisorApprovals",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorApprovals_ModifiedByUserId",
                table: "SupervisorApprovals",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorApprovals_RevokedBy",
                table: "SupervisorApprovals",
                column: "RevokedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorApprovals_RevokedByUserId",
                table: "SupervisorApprovals",
                column: "RevokedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorApprovals_UserId_DepartmentId_AcademicYearId",
                table: "SupervisorApprovals",
                columns: new[] { "UserId", "DepartmentId", "AcademicYearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CreatedBy",
                table: "Topics",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CreatedOn",
                table: "Topics",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_DeletedBy",
                table: "Topics",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_DirectionId",
                table: "Topics",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_DirectionsId",
                table: "Topics",
                column: "DirectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_ModifiedBy",
                table: "Topics",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_StatusId",
                table: "Topics",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_SupervisorId",
                table: "Topics",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_AssignedBy",
                table: "UserRoles",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RevokedBy",
                table: "UserRoles",
                column: "RevokedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RolesId",
                table: "UserRoles",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentsId",
                table: "Users",
                column: "DepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IIN",
                table: "Users",
                column: "IIN",
                unique: true,
                filter: "[IIN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastName_FirstName",
                table: "Users",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeId",
                table: "Users",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypesId",
                table: "Users",
                column: "UserTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkChecks_CheckedOn",
                table: "WorkChecks",
                column: "CheckedOn");

            migrationBuilder.CreateIndex(
                name: "IX_WorkChecks_CheckTypeId",
                table: "WorkChecks",
                column: "CheckTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkChecks_CreatedBy",
                table: "WorkChecks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkChecks_ExpertId",
                table: "WorkChecks",
                column: "ExpertId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkChecks_ModifiedBy",
                table: "WorkChecks",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkChecks_ReviewerId",
                table: "WorkChecks",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkChecks_StatusId",
                table: "WorkChecks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkChecks_StudentWorkId_CheckTypeId_StatusId",
                table: "WorkChecks",
                columns: new[] { "StudentWorkId", "CheckTypeId", "StatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkChecks_SubmittedOn",
                table: "WorkChecks",
                column: "SubmittedOn");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTypes_DeletedBy",
                table: "WorkTypes",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTypes_Name",
                table: "WorkTypes",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "DepartmentExperts");

            migrationBuilder.DropTable(
                name: "EvaluationScores");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SupervisorApprovals");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "WorkChecks");

            migrationBuilder.DropTable(
                name: "CommissionMembers");

            migrationBuilder.DropTable(
                name: "DefenseGrades");

            migrationBuilder.DropTable(
                name: "EvaluationCriteria");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CheckTypes");

            migrationBuilder.DropTable(
                name: "ExternalContacts");

            migrationBuilder.DropTable(
                name: "DefenseSchedules");

            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropTable(
                name: "StudentWork");

            migrationBuilder.DropTable(
                name: "CommissionTypes");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "WorkTypes");

            migrationBuilder.DropTable(
                name: "AcademicYears");

            migrationBuilder.DropTable(
                name: "PeriodTypes");

            migrationBuilder.DropTable(
                name: "Directions");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropTable(
                name: "Institutes");
        }
    }
}
