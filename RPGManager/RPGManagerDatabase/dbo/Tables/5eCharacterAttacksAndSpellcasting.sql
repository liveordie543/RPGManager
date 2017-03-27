CREATE TABLE [dbo].[5eCharacterAttacksAndSpellcasting]
(
	[CharacterId] INT NOT NULL , 
    [Name] NVARCHAR(64) NOT NULL, 
    [AttackBonus] INT NOT NULL, 
    [Damage] NVARCHAR(10) NOT NULL, 
    [DamageType] NVARCHAR(10) NOT NULL, 
    [Notes] NVARCHAR(256) NULL,
	CONSTRAINT [FK_5eCharacterAttacksAndSpellcasting_5eCharacters] FOREIGN KEY ([CharacterId]) REFERENCES [5eCharacters]([Id]) ON DELETE CASCADE
)
