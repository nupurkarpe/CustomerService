using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cus1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_kyc_customerId",
                table: "kyc");

            migrationBuilder.DropIndex(
                name: "IX_kyc_docTypeId",
                table: "kyc");

            migrationBuilder.CreateIndex(
                name: "IX_kyc_customerId",
                table: "kyc",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_kyc_docTypeId",
                table: "kyc",
                column: "docTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_kyc_customerId",
                table: "kyc");

            migrationBuilder.DropIndex(
                name: "IX_kyc_docTypeId",
                table: "kyc");

            migrationBuilder.CreateIndex(
                name: "IX_kyc_customerId",
                table: "kyc",
                column: "customerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kyc_docTypeId",
                table: "kyc",
                column: "docTypeId",
                unique: true);
        }
    }
}
