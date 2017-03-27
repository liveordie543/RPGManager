CREATE TABLE [dbo].[5eCharacterSavingThrows]
(
	[Id] INT NOT NULL , 
    [Strength] INT NOT NULL,
    [Dexterity] INT NOT NULL, 
    [Constitution] INT NOT NULL, 
    [Intelligence] INT NOT NULL, 
    [Wisdom] INT NOT NULL, 
    [Charisma] INT NOT NULL, 
    CONSTRAINT [PK_5eCharacterSavingThrows] PRIMARY KEY ([Id])
)
