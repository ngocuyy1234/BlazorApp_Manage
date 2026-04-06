using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp_Manage.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVlanStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Vlanid",
                table: "NetworkConfigs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vlanid",
                table: "NetworkConfigs");
        }
    }
}
