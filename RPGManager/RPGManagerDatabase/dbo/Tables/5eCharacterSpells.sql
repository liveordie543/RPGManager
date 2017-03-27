CREATE TABLE [dbo].[5eCharacterSpells]
(
	[CharacterId] INT NOT NULL , 
    [Level] INT NOT NULL, 
    [Name] NVARCHAR(64) NOT NULL, 
    [Description] NVARCHAR(256) NULL,
	CONSTRAINT [FK_5eCharacterSpells_5eCharacters] FOREIGN KEY ([CharacterId]) REFERENCES [5eCharacters]([Id]) ON DELETE CASCADE
)
