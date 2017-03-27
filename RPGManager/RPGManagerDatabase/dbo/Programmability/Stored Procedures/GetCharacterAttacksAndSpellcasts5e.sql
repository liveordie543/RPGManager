CREATE PROCEDURE [dbo].[GetCharacterAttacksAndSpellcasts5e]
	@CharacterId int
AS
	SELECT [CharacterId], [Name], [AttackBonus], [Damage], [DamageType], [Notes]
	FROM [5eCharacterAttacksAndSpellcasting]
	WHERE [CharacterId] = @CharacterId 

RETURN 0
