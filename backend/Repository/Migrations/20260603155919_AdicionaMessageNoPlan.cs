using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AdicionaMessageNoPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_CreateOpportunityActionPlan;
                DROP PROCEDURE IF EXISTS sp_GetOpportunityActionPlanById;
                ALTER TABLE OpportunityActionPlans
                DROP COLUMN IF EXISTS Message;
            ");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "OpportunityActionPlans",
                type: "nvarchar(max)",
                nullable: true);


            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_CreateOpportunityActionPlan
                    @OpportunityId INT,
                    @Message NVARCHAR(MAX),
                    @ActionPlan NVARCHAR(MAX),
                    @GeneratedAt DATETIME2(7)
                AS
                BEGIN
                    INSERT INTO OpportunityActionPlans
                    (
                        OpportunityId,
                        Message,
                        ActionPlan,
                        GeneratedAt
                    )
                    VALUES
                    (
                        @OpportunityId,
                        @Message,
                        @ActionPlan,
                        @GeneratedAt
                    );

                    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ID;
                END
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetOpportunityActionPlanById
                    @ID INT
                AS
                BEGIN
                    SELECT
                        Id,
                        OpportunityId,
                        Message,
                        ActionPlan,
                        GeneratedAt
                    FROM OpportunityActionPlans
                    WHERE Id = @ID;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "OpportunityActionPlans");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_CreateOpportunityActionPlan;
                DROP PROCEDURE IF EXISTS sp_GetOpportunityActionPlanById;
            ");
        }
    }
}