using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MESDemo.Migrations
{
    /// <inheritdoc />
    public partial class NewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DowntimeSeconds",
                table: "MachineData",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MachineHealth",
                table: "MachineData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OEE",
                table: "MachineData",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProcessTemperature",
                table: "MachineData",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductionRate",
                table: "MachineData",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Quality",
                table: "MachineData",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DowntimeSeconds",
                table: "MachineData");

            migrationBuilder.DropColumn(
                name: "MachineHealth",
                table: "MachineData");

            migrationBuilder.DropColumn(
                name: "OEE",
                table: "MachineData");

            migrationBuilder.DropColumn(
                name: "ProcessTemperature",
                table: "MachineData");

            migrationBuilder.DropColumn(
                name: "ProductionRate",
                table: "MachineData");

            migrationBuilder.DropColumn(
                name: "Quality",
                table: "MachineData");
        }
    }
}
