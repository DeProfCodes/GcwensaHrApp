using Microsoft.EntityFrameworkCore.Migrations;

namespace GcwensaHrApp.Migrations
{
    public partial class CreateRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "AspNetRoles",
                new string[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                new string[] { "1", "Admin", "Admin", "Admin" });

            migrationBuilder.InsertData(
                "AspNetRoles",
                new string[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                new string[] { "2", "Human Resources", "Human Resources", "Human Resources" });

            migrationBuilder.InsertData(
               "AspNetRoles",
               new string[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               new string[] { "3", "Employee", "Employee", "Employee" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
