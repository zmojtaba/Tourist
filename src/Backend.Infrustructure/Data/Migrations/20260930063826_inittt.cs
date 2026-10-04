using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrustructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class inittt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Doors_FacilityId_RoomNumber",
                table: "Doors",
                columns: new[] { "FacilityId", "RoomNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doors_FacilityId_RoomNumber",
                table: "Doors");
        }
    }
}
