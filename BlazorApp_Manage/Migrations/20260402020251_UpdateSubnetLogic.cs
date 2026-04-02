using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp_Manage.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubnetLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cidr",
                table: "VLANs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "VLANs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NetworkAddress",
                table: "VLANs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cidr",
                table: "VLANs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "VLANs");

            migrationBuilder.DropColumn(
                name: "NetworkAddress",
                table: "VLANs");
        }
    }
}
