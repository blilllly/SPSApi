using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSApi.Modules.Customers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Customers_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "customers");

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "customers",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Area_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "customers",
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.CheckConstraint("CK_Contact_CustomerOrBranch", "[CustomerId] IS NOT NULL OR [BranchId] IS NOT NULL");
                    table.ForeignKey(
                        name: "FK_Contact_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "customers",
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contact_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "customers",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Area_BranchId_Name",
                schema: "customers",
                table: "Area",
                columns: new[] { "BranchId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CustomerId_Name",
                schema: "customers",
                table: "Branch",
                columns: new[] { "CustomerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_BranchId",
                schema: "customers",
                table: "Contact",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CustomerId",
                schema: "customers",
                table: "Contact",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Name",
                schema: "customers",
                table: "Customer",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Area",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "Contact",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "Branch",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "customers");
        }
    }
}
