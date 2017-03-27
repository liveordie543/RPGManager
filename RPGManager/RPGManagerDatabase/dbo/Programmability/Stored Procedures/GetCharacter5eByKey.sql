CREATE PROCEDURE [dbo].[GetCharacter5eByKey]
	@Key uniqueidentifier
AS
	SELECT [Key], [Name], [Class], [Level], [Background], [PlayerName], [Race], [Alignment], [Experience], c.[Strength], c.[Dexterity], c.[Constitution]
		,c.[Intelligence], c.[Wisdom], c.[Charisma], [Inspiration], [ProficiencyBonus], [SavingThrowsId], [SkillsId], [ArmorClass], [Initiative], [Speed]
		,[HitPointMaximum], [CurrentHitPoints], [TemporaryHitPoints], [TotalHitDice], [HitDice], [SuccessDeathSaves], [FailedDeathSaves], [MoneyId]
		,[PersonalityTraits], [Ideals], [Bonds], [Flaws], [PassiveWisdom], [Age], [Height], [Weight], [Eyes], [Skin], [Hair], [SpellcastingClass]
		,[SpellcastingAbility], [SpellSaveDC], [SpellAttackBonus], [SpellSlotsId], [AppearanceKey], [Backstory], cm.CP, cm.SP, cm.EP, cm.GP, cm.PP
		,cst.Charisma as cstCharisma, cst.Constitution as cstConstitution, cst.Dexterity as cstDexterity, cst.Intelligence as cstIntelligence
		,cst.Strength as cstStrength, cst.Wisdom as cstWisdom, [Acrobatics], [AcrobaticsIsProficient], [AnimalHandling], [AnimalHandlingIsProficient]
		,[Arcana], [ArcanaIsProficient], [Athletics], [AthleticsIsProficient], [Deception], [DeceptionIsProficient], [History], [HistoryIsProficient]
		,[Insight], [InsightIsProficient], [Intimidation], [IntimidationIsProficient], [Investigation], [InvestigationIsProficient], [Medicine]
		,[MedicineIsProficient], [Nature], [NatureIsProficient], [Perception], [PerceptionIsProficient], [Performance], [PerformanceIsProficient], [Persuasion]
		,[PersuasionIsProficient], [Religion], [ReligionIsProficient], [SleightOfHand], [SleightOfHandIsProficient], [Stealth], [StealthIsProficient]
		,[Survival], [SurvivalIsProficient], [0Max], [0Used], [1Max], [1Used], [2Max], [2Used], [3Max], [3Used], [4Max], [4Used], [5Max], [5Used], [6Max], [6Used]
		,[7Max], [7Used], [8Max], [8Used], [9Max], [9Used]
	FROM [5eCharacters] as c
	INNER JOIN [5eCharacterMoney] as cm ON cm.Id = [MoneyId]
	INNER JOIN [5eCharacterSavingThrows] as cst ON cst.Id = [SavingThrowsId]
	INNER JOIN [5eCharacterSkills] as cs ON cs.Id = [SkillsId]
	INNER JOIN [5eCharacterSpellSlots] as csl ON csl.Id = [SpellSlotsId]
	WHERE [Key] = @Key

RETURN 0
