using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_cascade_for_cyclic_heat_days : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "CyclicHeatTaskId",
                schema: "Garages",
                table: "CyclicHeatTaskDay");

            migrationBuilder.DeleteData(
                schema: "Garages",
                table: "Garage",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddForeignKey(
                name: "CyclicHeatTaskId",
                schema: "Garages",
                table: "CyclicHeatTaskDay",
                column: "CyclicHeatTaskId",
                principalSchema: "Garages",
                principalTable: "CyclicHeatTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "CyclicHeatTaskId",
                schema: "Garages",
                table: "CyclicHeatTaskDay");

            migrationBuilder.InsertData(
                schema: "Garages",
                table: "Garage",
                columns: new[] { "Id", "Ip", "Name" },
                values: new object[] { 1, "192.168.1.10", "Garage Robert" });

            migrationBuilder.AddForeignKey(
                name: "CyclicHeatTaskId",
                schema: "Garages",
                table: "CyclicHeatTaskDay",
                column: "CyclicHeatTaskId",
                principalSchema: "Garages",
                principalTable: "CyclicHeatTask",
                principalColumn: "Id");
        }
    }
}
