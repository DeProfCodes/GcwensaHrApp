using Microsoft.EntityFrameworkCore.Migrations;

namespace GcwensaHrApp.Migrations
{
    public partial class AddLeaveRequestFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeaveStatus",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveStatus",
                table: "LeaveRequests");
        }
    }
}
