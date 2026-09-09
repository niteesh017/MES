using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MESDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddProductionOrderAndBatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatch_ProductionOrders_ProductionOrderId",
                table: "ProductionBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_Machines_MachineId",
                table: "ProductionOrders");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "ProductionOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatch_ProductionOrders_ProductionOrderId",
                table: "ProductionBatch",
                column: "ProductionOrderId",
                principalTable: "ProductionOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_Machines_MachineId",
                table: "ProductionOrders",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatch_ProductionOrders_ProductionOrderId",
                table: "ProductionBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_Machines_MachineId",
                table: "ProductionOrders");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ProductionOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatch_ProductionOrders_ProductionOrderId",
                table: "ProductionBatch",
                column: "ProductionOrderId",
                principalTable: "ProductionOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_Machines_MachineId",
                table: "ProductionOrders",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
