CREATE TABLE [dbo].[5eCharacterEquipment]
(
	[CharacterId] INT NOT NULL PRIMARY KEY, 
    [Quantity] INT NOT NULL, 
    [Name] NVARCHAR(64) NOT NULL,
	[Description] NVARCHAR(256) NULL,
    [Weight] INT NOT NULL, 
    [Value] INT NOT NULL
)
