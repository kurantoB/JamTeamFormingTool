using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JamTeamFormingTool.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleTemplates",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    IsStaged = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuthorizePassCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTemplates", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "CoverageSets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleTemplateName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageSets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CoverageSets_RoleTemplates_RoleTemplateName",
                        column: x => x.RoleTemplateName,
                        principalTable: "RoleTemplates",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JamTeamFormingSessions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleTemplateName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Info = table.Column<string>(type: "TEXT", maxLength: 120, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Phase = table.Column<int>(type: "INTEGER", nullable: false),
                    AdminEmail = table.Column<string>(type: "TEXT", nullable: false),
                    AdminPassCode = table.Column<string>(type: "TEXT", nullable: false),
                    GenericPassCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JamTeamFormingSessions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JamTeamFormingSessions_RoleTemplates_RoleTemplateName",
                        column: x => x.RoleTemplateName,
                        principalTable: "RoleTemplates",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleTemplateName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Roles_RoleTemplates_RoleTemplateName",
                        column: x => x.RoleTemplateName,
                        principalTable: "RoleTemplates",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverageSetRoleCategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoverageSetID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageSetRoleCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CoverageSetRoleCategories_CoverageSets_CoverageSetID",
                        column: x => x.CoverageSetID,
                        principalTable: "CoverageSets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Handle = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Portfolio = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Region = table.Column<int>(type: "INTEGER", nullable: true),
                    PassCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Participants_JamTeamFormingSessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "JamTeamFormingSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Handle = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Pitch = table.Column<string>(type: "TEXT", maxLength: 280, nullable: true),
                    Region = table.Column<int>(type: "INTEGER", nullable: true),
                    PassCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Teams_JamTeamFormingSessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "JamTeamFormingSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleCategoryRole",
                columns: table => new
                {
                    RoleCategoriesID = table.Column<int>(type: "INTEGER", nullable: false),
                    RolesID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleCategoryRole", x => new { x.RoleCategoriesID, x.RolesID });
                    table.ForeignKey(
                        name: "FK_RoleCategoryRole_CoverageSetRoleCategories_RoleCategoriesID",
                        column: x => x.RoleCategoriesID,
                        principalTable: "CoverageSetRoleCategories",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_RoleCategoryRole_Roles_RolesID",
                        column: x => x.RolesID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantRole",
                columns: table => new
                {
                    ParticipantsID = table.Column<int>(type: "INTEGER", nullable: false),
                    RolesID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantRole", x => new { x.ParticipantsID, x.RolesID });
                    table.ForeignKey(
                        name: "FK_ParticipantRole_Participants_ParticipantsID",
                        column: x => x.ParticipantsID,
                        principalTable: "Participants",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ParticipantRole_Roles_RolesID",
                        column: x => x.RolesID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamRole",
                columns: table => new
                {
                    OpenRolesID = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamsID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRole", x => new { x.OpenRolesID, x.TeamsID });
                    table.ForeignKey(
                        name: "FK_TeamRole_Roles_OpenRolesID",
                        column: x => x.OpenRolesID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamRole_Teams_TeamsID",
                        column: x => x.TeamsID,
                        principalTable: "Teams",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoverageSetRoleCategories_CoverageSetID",
                table: "CoverageSetRoleCategories",
                column: "CoverageSetID");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageSets_RoleTemplateName",
                table: "CoverageSets",
                column: "RoleTemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_JamTeamFormingSessions_RoleTemplateName",
                table: "JamTeamFormingSessions",
                column: "RoleTemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantRole_RolesID",
                table: "ParticipantRole",
                column: "RolesID");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_SessionID",
                table: "Participants",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleCategoryRole_RolesID",
                table: "RoleCategoryRole",
                column: "RolesID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleTemplateName",
                table: "Roles",
                column: "RoleTemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRole_TeamsID",
                table: "TeamRole",
                column: "TeamsID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SessionID",
                table: "Teams",
                column: "SessionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipantRole");

            migrationBuilder.DropTable(
                name: "RoleCategoryRole");

            migrationBuilder.DropTable(
                name: "TeamRole");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "CoverageSetRoleCategories");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "CoverageSets");

            migrationBuilder.DropTable(
                name: "JamTeamFormingSessions");

            migrationBuilder.DropTable(
                name: "RoleTemplates");
        }
    }
}
