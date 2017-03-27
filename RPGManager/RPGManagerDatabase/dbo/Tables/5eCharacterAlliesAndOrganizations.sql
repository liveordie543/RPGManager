CREATE TABLE [dbo].[5eCharacterAlliesAndOrganizations]
(
	[CharacterId] INT NOT NULL , 
    [Name] NVARCHAR(64) NOT NULL, 
    [Description] NVARCHAR(256) NULL, 
    [ImageKey] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [FK_5eCharacterAlliesAndOrganizations_5eCharacters] FOREIGN KEY ([CharacterId]) REFERENCES [5eCharacters]([Id]) ON DELETE CASCADE
)
