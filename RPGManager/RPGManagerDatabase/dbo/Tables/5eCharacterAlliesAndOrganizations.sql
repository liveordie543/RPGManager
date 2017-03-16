CREATE TABLE [dbo].[5eCharacterAlliesAndOrganizations]
(
	[CharacterId] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(64) NOT NULL, 
    [Description] NVARCHAR(256) NULL, 
    [ImageKey] UNIQUEIDENTIFIER NULL
)
