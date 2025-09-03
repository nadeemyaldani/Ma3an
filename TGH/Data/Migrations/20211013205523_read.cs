using Microsoft.EntityFrameworkCore.Migrations;

namespace TGH.Data.Migrations
{
    public partial class read : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CustomerRead",
                table: "Conversations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DonatorRead",
                table: "Conversations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerRead",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "DonatorRead",
                table: "Conversations");
        }
    }
}
