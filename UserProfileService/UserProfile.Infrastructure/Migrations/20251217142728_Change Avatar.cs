using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserProfile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvatarUrl",
                table: "UserProfiles",
                newName: "AvatarFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvatarFileId",
                table: "UserProfiles",
                newName: "AvatarUrl");
        }
    }
}
