using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MESDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddBatchProductionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BadQuantity",
                table: "ProductionBatch",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GoodQuantity",
                table: "ProductionBatch",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProducedQuantity",
                table: "ProductionBatch",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BadQuantity",
                table: "ProductionBatch");

            migrationBuilder.DropColumn(
                name: "GoodQuantity",
                table: "ProductionBatch");

            migrationBuilder.DropColumn(
                name: "ProducedQuantity",
                table: "ProductionBatch");
        }
    }
}
