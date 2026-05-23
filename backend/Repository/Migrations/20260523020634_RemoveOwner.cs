using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class RemoveOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Owners_OwnerId",
                table: "Opportunities");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_OwnerId",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Opportunities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Opportunities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Owners_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_OwnerId",
                table: "Opportunities",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_UserID",
                table: "Owners",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Owners_OwnerId",
                table: "Opportunities",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
