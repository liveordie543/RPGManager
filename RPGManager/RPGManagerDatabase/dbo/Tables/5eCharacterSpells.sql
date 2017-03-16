CREATE TABLE [dbo].[5eCharacterSpells]
(
	[CharacterId] INT NOT NULL PRIMARY KEY, 
    [Level] INT NOT NULL, 
    [Name] NVARCHAR(64) NOT NULL, 
    [Description] NVARCHAR(256) NULL
)
