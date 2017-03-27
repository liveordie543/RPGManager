CREATE PROCEDURE [dbo].[DeleteCharacter5eByKey]
	@Key uniqueidentifier
AS
	DELETE FROM [5eCharacters]
	WHERE [Key] = @Key

RETURN 0
