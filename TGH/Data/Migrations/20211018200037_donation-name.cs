using Microsoft.EntityFrameworkCore.Migrations;

namespace TGH.Data.Migrations
{
    public partial class donationname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowName",
                table: "Donations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowName",
                table: "Donations");
        }
    }
}
