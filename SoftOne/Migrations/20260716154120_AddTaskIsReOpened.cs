using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftOne.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskIsReOpened : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReOpened",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReOpened",
                table: "Tasks");
        }
    }
}
