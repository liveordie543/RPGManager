CREATE TABLE [dbo].[5eCharacterProficiencies]
(
	[CharacterId] INT NOT NULL PRIMARY KEY, 
    [Type] INT NOT NULL, 
    [Proficiency] NVARCHAR(64) NOT NULL
)
