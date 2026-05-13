using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class RenameOwersToOwners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Owers_OwnerId",
                table: "Opportunities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owers",
                table: "Owers");

            migrationBuilder.DropIndex(
                name: "IX_Owers_UserID",
                table: "Owers");

            migrationBuilder.DropForeignKey(
                name: "FK_Owers_Users_UserID",
                table: "Owers");

            migrationBuilder.RenameTable(
                name: "Owers",
                newName: "Owners");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owners",
                table: "Owners",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_UserID",
                table: "Owners",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Users_UserID",
                table: "Owners",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Owners_OwnerId",
                table: "Opportunities",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Owners_OwnerId",
                table: "Opportunities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owners",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Owners_UserID",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Users_UserID",
                table: "Owners");

            migrationBuilder.RenameTable(
                name: "Owners",
                newName: "Owers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owers",
                table: "Owers",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Owers_UserID",
                table: "Owers",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Owers_Users_UserID",
                table: "Owers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Owers_OwnerId",
                table: "Opportunities",
                column: "OwnerId",
                principalTable: "Owers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
