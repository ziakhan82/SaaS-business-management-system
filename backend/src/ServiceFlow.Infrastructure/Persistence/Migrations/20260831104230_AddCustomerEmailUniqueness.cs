using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerEmailUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_CompanyId_Email",
                table: "Customers");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId_Email",
                table: "Customers",
                columns: new[] { "CompanyId", "Email" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_CompanyId_Email",
                table: "Customers");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId_Email",
                table: "Customers",
                columns: new[] { "CompanyId", "Email" });
        }
    }
}
