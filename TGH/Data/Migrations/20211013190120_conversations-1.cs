using Microsoft.EntityFrameworkCore.Migrations;

namespace TGH.Data.Migrations
{
    public partial class conversations1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Conversations_CustomerId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_DonatorId",
                table: "Conversations");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CustomerId",
                table: "Conversations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_DonatorId",
                table: "Conversations",
                column: "DonatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Conversations_CustomerId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_DonatorId",
                table: "Conversations");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CustomerId",
                table: "Conversations",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_DonatorId",
                table: "Conversations",
                column: "DonatorId",
                unique: true,
                filter: "[DonatorId] IS NOT NULL");
        }
    }
}
