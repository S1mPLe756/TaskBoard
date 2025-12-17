using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changeattachmentscascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardAttachment_Cards_CardId",
                table: "CardAttachment");

            migrationBuilder.AlterColumn<Guid>(
                name: "CardId",
                table: "CardAttachment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CardAttachment_Cards_CardId",
                table: "CardAttachment",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardAttachment_Cards_CardId",
                table: "CardAttachment");

            migrationBuilder.AlterColumn<Guid>(
                name: "CardId",
                table: "CardAttachment",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CardAttachment_Cards_CardId",
                table: "CardAttachment",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }
    }
}
