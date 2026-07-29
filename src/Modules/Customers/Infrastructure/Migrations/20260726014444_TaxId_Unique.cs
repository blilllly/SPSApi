using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSApi.Modules.Customers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TaxId_Unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Customer_TaxId",
                schema: "customers",
                table: "Customer",
                column: "TaxId",
                unique: true,
                filter: "[TaxId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customer_TaxId",
                schema: "customers",
                table: "Customer");
        }
    }
}
