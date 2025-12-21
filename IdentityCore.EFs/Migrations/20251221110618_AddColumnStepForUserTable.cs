using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityCore.EFs.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnStepForUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Step",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Step",
                table: "Users");
        }
    }
}
