using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarciaApi.Migrations
{
    /// <inheritdoc />
    public partial class ninth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "RolesUserModel",
                columns: table => new
                {
                    RolesRoleId = table.Column<string>(type: "text", nullable: false),
                    UserModelsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUserModel", x => new { x.RolesRoleId, x.UserModelsId });
                    table.ForeignKey(
                        name: "FK_RolesUserModel_Roles_RolesRoleId",
                        column: x => x.RolesRoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesUserModel_Users_UserModelsId",
                        column: x => x.UserModelsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolesUserModel_UserModelsId",
                table: "RolesUserModel",
                column: "UserModelsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesUserModel");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.AddColumn<List<string>>(
                name: "Roles",
                table: "Users",
                type: "text[]",
                nullable: false);
        }
    }
}
