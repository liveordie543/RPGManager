CREATE TABLE [dbo].[5eCharacterProficiencies]
(
	[CharacterId] INT NOT NULL , 
    [Type] INT NOT NULL, 
    [Proficiency] NVARCHAR(64) NOT NULL,
	CONSTRAINT [FK_5eCharacterProficiencies_5eCharacters] FOREIGN KEY ([CharacterId]) REFERENCES [5eCharacters]([Id]) ON DELETE CASCADE
)
