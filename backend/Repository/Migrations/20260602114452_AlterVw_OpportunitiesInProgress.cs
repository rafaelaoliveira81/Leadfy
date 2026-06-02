using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AlterVw_OpportunitiesInProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.vw_OpportunitiesInProgress', 'V') IS NOT NULL
                    DROP VIEW dbo.vw_OpportunitiesInProgress;
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW dbo.vw_OpportunitiesInProgress
                AS
                SELECT
                    COUNT(*) AS OpportunitiesInProgress
                FROM Opportunities
                WHERE Stage NOT IN (1,6,7);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.vw_OpportunitiesInProgress', 'V') IS NOT NULL
                    DROP VIEW dbo.vw_OpportunitiesInProgress;
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW dbo.vw_OpportunitiesInProgress
                AS
                SELECT
                    COUNT(*) AS OpportunitiesInProgress
                FROM Opportunities
                WHERE Stage NOT IN (6,7);
            ");
        }
    }
}