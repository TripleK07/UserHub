using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class menu_add_controller_action_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionName",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ControllerName",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionName",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "ControllerName",
                table: "Menus");
        }
    }
}
