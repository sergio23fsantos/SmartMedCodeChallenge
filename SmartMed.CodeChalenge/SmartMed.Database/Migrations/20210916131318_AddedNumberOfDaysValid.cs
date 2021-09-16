using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartMed.Database.Migrations
{
    public partial class AddedNumberOfDaysValid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfDaysValid",
                table: "Medications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfDaysValid",
                table: "Medications");
        }
    }
}
