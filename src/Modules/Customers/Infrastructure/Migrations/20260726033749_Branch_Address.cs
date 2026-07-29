using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSApi.Modules.Customers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Branch_Address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "customers",
                table: "Branch");

            migrationBuilder.AddColumn<string>(
                name: "BuildingNumber",
                schema: "customers",
                table: "Branch",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "customers",
                table: "Branch",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainStreet",
                schema: "customers",
                table: "Branch",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "customers",
                table: "Branch",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                schema: "customers",
                table: "Branch",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryStreet",
                schema: "customers",
                table: "Branch",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingNumber",
                schema: "customers",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "City",
                schema: "customers",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "MainStreet",
                schema: "customers",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "customers",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "Province",
                schema: "customers",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "SecondaryStreet",
                schema: "customers",
                table: "Branch");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "customers",
                table: "Branch",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
