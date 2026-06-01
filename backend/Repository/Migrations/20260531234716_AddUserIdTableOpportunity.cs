using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddUserIdTableOpportunity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordRecoveries");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Opportunities");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_UserId",
                table: "Opportunities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Users_UserId",
                table: "Opportunities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Users_UserId",
                table: "Opportunities");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_UserId",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Opportunities");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Opportunities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PasswordRecoveries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRecoveries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PasswordRecoveries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordRecoveries_Token",
                table: "PasswordRecoveries",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordRecoveries_UserId",
                table: "PasswordRecoveries",
                column: "UserId");
        }
    }
}
