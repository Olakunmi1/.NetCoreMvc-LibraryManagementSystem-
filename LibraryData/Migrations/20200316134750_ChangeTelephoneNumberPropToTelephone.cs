using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryData.Migrations
{
    public partial class ChangeTelephoneNumberPropToTelephone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelephoneNumber",
                table: "Patrons");

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "Patrons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "Patrons");

            migrationBuilder.AddColumn<string>(
                name: "TelephoneNumber",
                table: "Patrons",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
