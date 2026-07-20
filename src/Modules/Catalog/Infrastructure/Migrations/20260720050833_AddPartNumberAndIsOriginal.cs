using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSApi.Modules.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartNumberAndIsOriginal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOriginal",
                schema: "catalog",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PartNumber",
                schema: "catalog",
                table: "Product",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartNumber",
                schema: "catalog",
                table: "AssetModel",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_PartNumber",
                schema: "catalog",
                table: "Product",
                column: "PartNumber",
                filter: "[PartNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_PartNumber",
                schema: "catalog",
                table: "AssetModel",
                column: "PartNumber",
                filter: "[PartNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_PartNumber",
                schema: "catalog",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_PartNumber",
                schema: "catalog",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "IsOriginal",
                schema: "catalog",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PartNumber",
                schema: "catalog",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PartNumber",
                schema: "catalog",
                table: "AssetModel");
        }
    }
}
