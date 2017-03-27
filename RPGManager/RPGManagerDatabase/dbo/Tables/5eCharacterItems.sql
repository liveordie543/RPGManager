CREATE TABLE [dbo].[5eCharacterEquipment]
(
	[CharacterId] INT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [Name] NVARCHAR(64) NOT NULL,
	[Description] NVARCHAR(256) NULL,
    [Weight] INT NOT NULL, 
    [Value] INT NOT NULL,
	CONSTRAINT [FK_5eCharacterEquipment_5eCharacters] FOREIGN KEY ([CharacterId]) REFERENCES [5eCharacters]([Id]) ON DELETE CASCADE
)
