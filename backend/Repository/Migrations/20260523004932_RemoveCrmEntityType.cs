using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class RemoveCrmEntityType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Interactions_CrmEntityType_CrmEntityId",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "CrmEntityId",
                table: "Interactions");

            migrationBuilder.RenameColumn(
                name: "CrmEntityType",
                table: "Interactions",
                newName: "OpportunityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpportunityId",
                table: "Interactions",
                newName: "CrmEntityType");

            migrationBuilder.AddColumn<int>(
                name: "CrmEntityId",
                table: "Interactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_CrmEntityType_CrmEntityId",
                table: "Interactions",
                columns: new[] { "CrmEntityType", "CrmEntityId" });
        }
    }
}
