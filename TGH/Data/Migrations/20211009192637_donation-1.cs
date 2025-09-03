using Microsoft.EntityFrameworkCore.Migrations;

namespace TGH.Data.Migrations
{
    public partial class donation1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donation_AspNetUsers_UserId",
                table: "Donation");

            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Category_CategoryId",
                table: "Donation");

            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Cities_CityId",
                table: "Donation");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationImage_Donation_DonationId",
                table: "DonationImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonationImage",
                table: "DonationImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Donation",
                table: "Donation");

            migrationBuilder.RenameTable(
                name: "DonationImage",
                newName: "DonationImages");

            migrationBuilder.RenameTable(
                name: "Donation",
                newName: "Donations");

            migrationBuilder.RenameIndex(
                name: "IX_DonationImage_DonationId",
                table: "DonationImages",
                newName: "IX_DonationImages_DonationId");

            migrationBuilder.RenameIndex(
                name: "IX_Donation_UserId",
                table: "Donations",
                newName: "IX_Donations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Donation_CityId",
                table: "Donations",
                newName: "IX_Donations_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Donation_CategoryId",
                table: "Donations",
                newName: "IX_Donations_CategoryId");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Donations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonationImages",
                table: "DonationImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Donations",
                table: "Donations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationImages_Donations_DonationId",
                table: "DonationImages",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Category_CategoryId",
                table: "Donations",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Cities_CityId",
                table: "Donations",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationImages_Donations_DonationId",
                table: "DonationImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Category_CategoryId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Cities_CityId",
                table: "Donations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Donations",
                table: "Donations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonationImages",
                table: "DonationImages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Donations");

            migrationBuilder.RenameTable(
                name: "Donations",
                newName: "Donation");

            migrationBuilder.RenameTable(
                name: "DonationImages",
                newName: "DonationImage");

            migrationBuilder.RenameIndex(
                name: "IX_Donations_UserId",
                table: "Donation",
                newName: "IX_Donation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Donations_CityId",
                table: "Donation",
                newName: "IX_Donation_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Donations_CategoryId",
                table: "Donation",
                newName: "IX_Donation_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_DonationImages_DonationId",
                table: "DonationImage",
                newName: "IX_DonationImage_DonationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Donation",
                table: "Donation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonationImage",
                table: "DonationImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_AspNetUsers_UserId",
                table: "Donation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Category_CategoryId",
                table: "Donation",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Cities_CityId",
                table: "Donation",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonationImage_Donation_DonationId",
                table: "DonationImage",
                column: "DonationId",
                principalTable: "Donation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
