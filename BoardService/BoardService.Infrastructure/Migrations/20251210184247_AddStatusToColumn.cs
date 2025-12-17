using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BoardColumns",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BoardColumns");
        }
    }
}
