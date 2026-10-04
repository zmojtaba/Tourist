using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrustructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class iiinittt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cameras_FacilityId_Name_Url",
                table: "Cameras",
                columns: new[] { "FacilityId", "Name", "Url" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cameras_FacilityId_Name_Url",
                table: "Cameras");
        }
    }
}
