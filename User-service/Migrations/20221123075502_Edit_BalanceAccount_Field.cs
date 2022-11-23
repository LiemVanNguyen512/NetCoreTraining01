using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_service.Migrations
{
    public partial class Edit_BalanceAccount_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "BalanceAccount",
                table: "User",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BalanceAccount",
                table: "User",
                type: "double",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");
        }
    }
}
