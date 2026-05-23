using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class StoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // AiConfig
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateAiConfig]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateAiConfig];
EXEC(N'
CREATE PROCEDURE sp_CreateAiConfig
    @Title NVARCHAR(150),
    @PromptTemplate NVARCHAR(2000),
    @ApiKeyHash NVARCHAR(512),
    @Model INT
AS
BEGIN
    INSERT INTO AiConfigs (Title, PromptTemplate, ApiKeyHash, Model, IsActive, CreatedAt)
    VALUES (@Title, @PromptTemplate, @ApiKeyHash, @Model, 1, GETDATE());

    SELECT SCOPE_IDENTITY() AS ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetAiConfigById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAiConfigById];
EXEC(N'
CREATE PROCEDURE sp_GetAiConfigById
    @ID INT
AS
BEGIN
    SELECT 
        ID,
        Title,
        PromptTemplate,
        ApiKeyHash,
        Model,
        IsActive,
        CreatedAt
    FROM AiConfigs
    WHERE ID = @ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetAllAiConfigs]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAllAiConfigs];
EXEC(N'
CREATE PROCEDURE sp_GetAllAiConfigs
    @IsActive BIT = NULL
AS
BEGIN
    SELECT 
        ID,
        Title,
        PromptTemplate,
        ApiKeyHash,
        Model,
        IsActive,
        CreatedAt
    FROM AiConfigs
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY CreatedAt DESC;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdateAiConfig]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateAiConfig];
EXEC(N'
CREATE PROCEDURE sp_UpdateAiConfig
    @ID INT,
    @Title NVARCHAR(150),
    @PromptTemplate NVARCHAR(2000),
    @ApiKeyHash NVARCHAR(512),
    @Model INT,
    @IsActive BIT
AS
BEGIN
    UPDATE AiConfigs
    SET 
        Title = @Title,
        PromptTemplate = @PromptTemplate,
        ApiKeyHash = @ApiKeyHash,
        Model = @Model,
        IsActive = @IsActive
    WHERE ID = @ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_DeleteAiConfig]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_DeleteAiConfig];
EXEC(N'
CREATE PROCEDURE sp_DeleteAiConfig
    @ID INT
AS
BEGIN
    DELETE FROM AiConfigs
    WHERE ID = @ID;
END
');");

            // Interaction (simplified bodies)
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateInteraction]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateInteraction];
EXEC(N'
CREATE PROCEDURE sp_CreateInteraction
    @OpportunityId INT,
    @FromStage INT,
    @ToStage INT,
    @Description NVARCHAR(1000),
    @UserID INT,
    @NextContactDate DATETIME2(7),
    @InteractionDate DATETIME2(7)
AS
BEGIN
    INSERT INTO Interactions (OpportunityId, FromStage, ToStage, Description, InteractionDate, UserID, NextContactDate, CreatedAt)
    VALUES (@OpportunityId, @FromStage, @ToStage, @Description, @InteractionDate, @UserID, @NextContactDate, GETDATE());
    SELECT SCOPE_IDENTITY();
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetInteractionById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetInteractionById];
EXEC(N'
CREATE PROCEDURE sp_GetInteractionById
    @ID INT
AS
BEGIN
    SELECT * FROM Interactions WHERE Id = @ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetAllInteractions]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAllInteractions];
EXEC(N'
CREATE PROCEDURE sp_GetAllInteractions
AS
BEGIN
    SELECT * FROM Interactions ORDER BY InteractionDate DESC;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdateInteraction]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateInteraction];
EXEC(N'
CREATE PROCEDURE sp_UpdateInteraction
    @ID INT,
    @Description NVARCHAR(1000),
    @NextContactDate DATETIME2(7)
AS
BEGIN
    UPDATE Interactions SET Description = @Description, NextContactDate = @NextContactDate WHERE Id = @ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_DeleteInteraction]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_DeleteInteraction];
EXEC(N'
CREATE PROCEDURE sp_DeleteInteraction
    @ID INT
AS
BEGIN
    DELETE FROM Interactions WHERE Id = @ID;
END
');");

            // Lead
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateLead]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateLead];
EXEC(N'
CREATE PROCEDURE sp_CreateLead
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @PhoneNumber NVARCHAR(50)
AS
BEGIN
    INSERT INTO Leads (Name, Email, PhoneNumber, IsActive, CreatedAt)
    VALUES (@Name, @Email, @PhoneNumber, 1, GETUTCDATE());
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetLeadById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetLeadById];
EXEC(N'
CREATE PROCEDURE sp_GetLeadById
    @Id INT
AS
BEGIN
    SELECT * FROM Leads WHERE Id = @Id;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetAllLeads]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAllLeads];
EXEC(N'
CREATE PROCEDURE sp_GetAllLeads
    @IsActive BIT = NULL
AS
BEGIN
    SELECT * FROM Leads WHERE (@IsActive IS NULL OR IsActive = @IsActive) ORDER BY CreatedAt DESC;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdateLead]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateLead];
EXEC(N'
CREATE PROCEDURE sp_UpdateLead
    @Id INT,
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @PhoneNumber NVARCHAR(50),
    @IsActive BIT
AS
BEGIN
    UPDATE Leads SET Name=@Name, Email=@Email, PhoneNumber=@PhoneNumber, IsActive=@IsActive WHERE Id=@Id;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_DeleteLead]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_DeleteLead];
EXEC(N'
CREATE PROCEDURE sp_DeleteLead
    @Id INT
AS
BEGIN
    DELETE FROM Leads WHERE Id = @Id;
END
');");

            // Opportunity
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateOpportunity]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateOpportunity];
EXEC(N'
CREATE PROCEDURE sp_CreateOpportunity
    @LeadId INT,
    @OwnerId INT = NULL,
    @ProductId INT,
    @Stage INT,
    @Amount DECIMAL(18,2),
    @ExpectedCloseDate DATETIME,
    @SortOrder INT
AS
BEGIN
    INSERT INTO Opportunities (LeadId, OwnerId, ProductId, Stage, Amount, ExpectedCloseDate, CreatedAt, IsActive, SortOrder)
    VALUES (@LeadId, @OwnerId, @ProductId, @Stage, @Amount, @ExpectedCloseDate, GETUTCDATE(), 1, @SortOrder);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetOpportunityById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetOpportunityById];
EXEC(N'
CREATE PROCEDURE sp_GetOpportunityById
    @ID INT
AS
BEGIN
    SELECT * FROM Opportunities WHERE ID = @ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetAllOpportunities]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAllOpportunities];
EXEC(N'
CREATE PROCEDURE sp_GetAllOpportunities
    @IsActive BIT = NULL
AS
BEGIN
    SELECT * FROM Opportunities WHERE (@IsActive IS NULL OR IsActive = @IsActive) ORDER BY CreatedAt DESC;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdateOpportunity]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateOpportunity];
EXEC(N'
CREATE PROCEDURE sp_UpdateOpportunity
    @ID INT,
    @LeadId INT,
    @OwnerId INT = NULL,
    @ProductId INT,
    @Stage INT,
    @Amount DECIMAL(18,2),
    @ExpectedCloseDate DATETIME,
    @SortOrder INT,
    @IsActive BIT
AS
BEGIN
    UPDATE Opportunities
    SET LeadId=@LeadId, OwnerId=@OwnerId, ProductId=@ProductId, Stage=@Stage, Amount=@Amount, ExpectedCloseDate=@ExpectedCloseDate, SortOrder=@SortOrder, IsActive=@IsActive
    WHERE ID=@ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_DeleteOpportunity]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_DeleteOpportunity];
EXEC(N'
CREATE PROCEDURE sp_DeleteOpportunity
    @ID INT
AS
BEGIN
    DELETE FROM Opportunities WHERE ID = @ID;
END
');");

            // OpportunityActionPlan
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateOpportunityActionPlan]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateOpportunityActionPlan];
EXEC(N'
CREATE PROCEDURE sp_CreateOpportunityActionPlan
    @OpportunityId INT,
    @AiConfigId INT,
    @ActionPlan NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO OpportunityActionPlans (OpportunityId, AiConfigId, ActionPlan, GeneratedAt) VALUES (@OpportunityId, @AiConfigId, @ActionPlan, GETUTCDATE());
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetOpportunityActionPlanById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetOpportunityActionPlanById];
EXEC(N'
CREATE PROCEDURE sp_GetOpportunityActionPlanById
    @ID INT
AS
BEGIN
    SELECT * FROM OpportunityActionPlans WHERE ID = @ID;
END
');");

            // Owner
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateOwner]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateOwner];
EXEC(N'
CREATE PROCEDURE sp_CreateOwner
    @Name NVARCHAR(255),
    @UserID INT
AS
BEGIN
    INSERT INTO Owners (Name, UserID, IsActive, CreatedAt) VALUES (@Name, @UserID, 1, GETUTCDATE());
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetOwnerById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetOwnerById];
EXEC(N'
CREATE PROCEDURE sp_GetOwnerById
    @ID INT
AS
BEGIN
    SELECT * FROM Owners WHERE ID = @ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetAllOwners]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAllOwners];
EXEC(N'
CREATE PROCEDURE sp_GetAllOwners
AS
BEGIN
    SELECT * FROM Owners ORDER BY CreatedAt DESC;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdateOwner]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateOwner];
EXEC(N'
CREATE PROCEDURE sp_UpdateOwner
    @ID INT,
    @Name NVARCHAR(255),
    @UserID INT,
    @IsActive BIT
AS
BEGIN
    UPDATE Owners SET Name=@Name, UserID=@UserID, IsActive=@IsActive WHERE ID=@ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_DeleteOwner]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_DeleteOwner];
EXEC(N'
CREATE PROCEDURE sp_DeleteOwner
    @ID INT
AS
BEGIN
    DELETE FROM Owners WHERE ID = @ID;
END
');");

            // PasswordRecovery
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreatePasswordRecovery]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreatePasswordRecovery];
EXEC(N'
CREATE PROCEDURE sp_CreatePasswordRecovery
    @UserId INT,
    @Token NVARCHAR(512),
    @ExpiresAt DATETIME2(7)
AS
BEGIN
    INSERT INTO PasswordRecoveries (UserId, Token, ExpiresAt, CreatedAt) VALUES (@UserId, @Token, @ExpiresAt, GETUTCDATE());
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetPasswordRecoveryById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetPasswordRecoveryById];
EXEC(N'
CREATE PROCEDURE sp_GetPasswordRecoveryById
    @ID INT
AS
BEGIN
    SELECT * FROM PasswordRecoveries WHERE ID = @ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetAllPasswordRecoveries]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAllPasswordRecoveries];
EXEC(N'
CREATE PROCEDURE sp_GetAllPasswordRecoveries
AS
BEGIN
    SELECT * FROM PasswordRecoveries ORDER BY CreatedAt DESC;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdatePasswordRecovery]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdatePasswordRecovery];
EXEC(N'
CREATE PROCEDURE sp_UpdatePasswordRecovery
    @ID INT,
    @Token NVARCHAR(512),
    @ExpiresAt DATETIME2(7)
AS
BEGIN
    UPDATE PasswordRecoveries SET Token=@Token, ExpiresAt=@ExpiresAt WHERE ID=@ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_DeletePasswordRecovery]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_DeletePasswordRecovery];
EXEC(N'
CREATE PROCEDURE sp_DeletePasswordRecovery
    @ID INT
AS
BEGIN
    DELETE FROM PasswordRecoveries WHERE ID = @ID;
END
');");

            // Product
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateProduct]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateProduct];
EXEC(N'
CREATE PROCEDURE sp_CreateProduct
    @Name NVARCHAR(255),
    @Price DECIMAL(18,2)
AS
BEGIN
    INSERT INTO Products (Name, Price, IsActive, CreatedAt) VALUES (@Name, @Price, 1, GETUTCDATE());
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetProductById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetProductById];
EXEC(N'
CREATE PROCEDURE sp_GetProductById
    @ID INT
AS
BEGIN
    SELECT * FROM Products WHERE ID = @ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetAllProducts]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAllProducts];
EXEC(N'
CREATE PROCEDURE sp_GetAllProducts
AS
BEGIN
    SELECT * FROM Products ORDER BY CreatedAt DESC;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdateProduct]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateProduct];
EXEC(N'
CREATE PROCEDURE sp_UpdateProduct
    @ID INT,
    @Name NVARCHAR(255),
    @Price DECIMAL(18,2),
    @IsActive BIT
AS
BEGIN
    UPDATE Products SET Name=@Name, Price=@Price, IsActive=@IsActive WHERE ID=@ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_DeleteProduct]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_DeleteProduct];
EXEC(N'
CREATE PROCEDURE sp_DeleteProduct
    @ID INT
AS
BEGIN
    DELETE FROM Products WHERE ID = @ID;
END
');");

            // User
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateUser]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateUser];
EXEC(N'
CREATE PROCEDURE sp_CreateUser
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(512)
AS
BEGIN
    INSERT INTO Users (Name, Email, PasswordHash, IsActive, CreatedAt) VALUES (@Name, @Email, @PasswordHash, 1, GETUTCDATE());
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
');");
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetUserById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetUserById];
EXEC(N'
CREATE PROCEDURE sp_GetUserById
    @ID INT
AS
BEGIN
    SELECT * FROM Users WHERE ID = @ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetUserByEmail]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetUserByEmail];
EXEC(N'
CREATE PROCEDURE sp_GetUserByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SELECT * FROM Users WHERE Email = @Email;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_GetAllUsers]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAllUsers];
EXEC(N'
CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SELECT * FROM Users ORDER BY CreatedAt DESC;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdateUser]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateUser];
EXEC(N'
CREATE PROCEDURE sp_UpdateUser
    @ID INT,
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @IsActive BIT
AS
BEGIN
    UPDATE Users SET Name=@Name, Email=@Email, IsActive=@IsActive WHERE ID=@ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_DeleteUser]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_DeleteUser];
EXEC(N'
CREATE PROCEDURE sp_DeleteUser
    @ID INT
AS
BEGIN
    DELETE FROM Users WHERE ID = @ID;
END
');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop all stored procedures created in Up
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_DeleteUser]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_DeleteUser];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_UpdateUser]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_UpdateUser];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetAllUsers]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetAllUsers];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetUserByEmail]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetUserByEmail];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetUserById]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetUserById];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_CreateUser]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_CreateUser];");

            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_DeleteProduct]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_DeleteProduct];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_UpdateProduct]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_UpdateProduct];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetAllProducts]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetAllProducts];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetProductById]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetProductById];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_CreateProduct]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_CreateProduct];");

            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_DeletePasswordRecovery]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_DeletePasswordRecovery];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_UpdatePasswordRecovery]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_UpdatePasswordRecovery];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetAllPasswordRecoveries]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetAllPasswordRecoveries];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetPasswordRecoveryById]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetPasswordRecoveryById];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_CreatePasswordRecovery]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_CreatePasswordRecovery];");

            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_DeleteOwner]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_DeleteOwner];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_UpdateOwner]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_UpdateOwner];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetAllOwners]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetAllOwners];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetOwnerById]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetOwnerById];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_CreateOwner]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_CreateOwner];");

            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetOpportunityActionPlanById]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetOpportunityActionPlanById];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_CreateOpportunityActionPlan]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_CreateOpportunityActionPlan];");

            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_DeleteOpportunity]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_DeleteOpportunity];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_UpdateOpportunity]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_UpdateOpportunity];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetAllOpportunities]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetAllOpportunities];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetOpportunityById]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetOpportunityById];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_CreateOpportunity]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_CreateOpportunity];");

            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_DeleteLead]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_DeleteLead];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_UpdateLead]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_UpdateLead];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetAllLeads]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetAllLeads];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetLeadById]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetLeadById];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_CreateLead]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_CreateLead];");

            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_DeleteInteraction]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_DeleteInteraction];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_UpdateInteraction]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_UpdateInteraction];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetAllInteractions]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetAllInteractions];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetInteractionById]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetInteractionById];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_CreateInteraction]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_CreateInteraction];");

            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_DeleteAiConfig]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_DeleteAiConfig];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_UpdateAiConfig]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_UpdateAiConfig];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetAllAiConfigs]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetAllAiConfigs];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_GetAiConfigById]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_GetAiConfigById];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[sp_CreateAiConfig]', N'P') IS NOT NULL DROP PROCEDURE [dbo].[sp_CreateAiConfig];");
        }
    }
}