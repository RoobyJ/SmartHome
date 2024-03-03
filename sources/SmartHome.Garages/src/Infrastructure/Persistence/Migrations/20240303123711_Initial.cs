using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Garages");

            migrationBuilder.CreateTable(
                name: "Garage",
                schema: "Garages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Ip = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Garage_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeatLog",
                schema: "Garages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Info = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("HeatLog_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CyclicHeatTask",
                schema: "Garages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GarageId = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CyclicHeatTask_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "GarageId",
                        column: x => x.GarageId,
                        principalSchema: "Garages",
                        principalTable: "Garage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HeatTask",
                schema: "Garages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GarageId = table.Column<int>(type: "integer", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("HeatTask_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "GarageId",
                        column: x => x.GarageId,
                        principalSchema: "Garages",
                        principalTable: "Garage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OutsideTemperature",
                schema: "Garages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    GarageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("OutsideTemperature_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "GarageId",
                        column: x => x.GarageId,
                        principalSchema: "Garages",
                        principalTable: "Garage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CyclicHeatTaskDay",
                schema: "Garages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    CyclicHeatTaskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CyclicHeatTaskDaysInWeek_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "CyclicHeatTaskId",
                        column: x => x.CyclicHeatTaskId,
                        principalSchema: "Garages",
                        principalTable: "CyclicHeatTask",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Garages",
                table: "Garage",
                columns: new[] { "Id", "Ip", "Name" },
                values: new object[] { 1, "192.168.1.10", "Garage Robert" });

            migrationBuilder.CreateIndex(
                name: "IX_CyclicHeatTask_GarageId",
                schema: "Garages",
                table: "CyclicHeatTask",
                column: "GarageId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicHeatTaskDaysInWeek_CyclicHeatTaskId",
                schema: "Garages",
                table: "CyclicHeatTaskDay",
                column: "CyclicHeatTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CyclicHeatTaskDaysInWeek_DayId",
                schema: "Garages",
                table: "CyclicHeatTaskDay",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_HeatTask_GarageId",
                schema: "Garages",
                table: "HeatTask",
                column: "GarageId");

            migrationBuilder.CreateIndex(
                name: "IX_OutsideTemperature_GarageId",
                schema: "Garages",
                table: "OutsideTemperature",
                column: "GarageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CyclicHeatTaskDay",
                schema: "Garages");

            migrationBuilder.DropTable(
                name: "HeatLog",
                schema: "Garages");

            migrationBuilder.DropTable(
                name: "HeatTask",
                schema: "Garages");

            migrationBuilder.DropTable(
                name: "OutsideTemperature",
                schema: "Garages");

            migrationBuilder.DropTable(
                name: "CyclicHeatTask",
                schema: "Garages");

            migrationBuilder.DropTable(
                name: "Garage",
                schema: "Garages");
        }
    }
}
