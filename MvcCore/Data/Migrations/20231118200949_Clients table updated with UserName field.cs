using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcCore.Data.Migrations
{
    public partial class ClientstableupdatedwithUserNamefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Clients");
        }
    }
}
