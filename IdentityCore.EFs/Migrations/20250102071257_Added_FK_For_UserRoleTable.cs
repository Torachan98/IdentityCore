using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityCore.EFs.Migrations
{
    /// <inheritdoc />
    public partial class Added_FK_For_UserRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RolesRoleId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RolesRoleId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "RolesRoleId",
                table: "UserRoles");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles");

            migrationBuilder.AddColumn<int>(
                name: "RolesRoleId",
                table: "UserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RolesRoleId",
                table: "UserRoles",
                column: "RolesRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RolesRoleId",
                table: "UserRoles",
                column: "RolesRoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
