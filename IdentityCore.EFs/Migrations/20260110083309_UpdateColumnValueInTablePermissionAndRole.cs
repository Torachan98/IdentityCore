using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityCore.EFs.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnValueInTablePermissionAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PermissionType",
                table: "Permissions",
                newName: "Value");

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "RoleName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Permissions",
                newName: "PermissionType");
        }
    }
}
