using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankManagementSystemVersionFinal1.Data.Migrations
{
    /// <inheritdoc />
    public partial class bankdb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Employees");
        }
    }
}
