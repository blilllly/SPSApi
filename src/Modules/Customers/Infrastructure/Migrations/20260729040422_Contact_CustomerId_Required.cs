using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSApi.Modules.Customers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Contact_CustomerId_Required : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Contact_CustomerOrBranch",
                schema: "customers",
                table: "Contact");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                schema: "customers",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                schema: "customers",
                table: "Contact",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Contact_CustomerOrBranch",
                schema: "customers",
                table: "Contact",
                sql: "[CustomerId] IS NOT NULL OR [BranchId] IS NOT NULL");
        }
    }
}
