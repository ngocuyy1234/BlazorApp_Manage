using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp_Manage.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeviceUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ports_Vlanid",
                table: "Ports",
                column: "Vlanid");

            migrationBuilder.AddForeignKey(
                name: "FK_Ports_VLANs_Vlanid",
                table: "Ports",
                column: "Vlanid",
                principalTable: "VLANs",
                principalColumn: "VLANID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ports_VLANs_Vlanid",
                table: "Ports");

            migrationBuilder.DropIndex(
                name: "IX_Ports_Vlanid",
                table: "Ports");
        }
    }
}
