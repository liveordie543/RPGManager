using System;
using System.Collections.Generic;

namespace RPGManager.Data.Models
{
    public class Character5e : DbEntity, ICharacter
    {
        public Character5e()
        {
            AttacksAndSpellcasts = new List<CharacterAttackOrSpellcast5e>();
            Items = new List<CharacterItem5e>();
            FeaturesAndTraits = new List<CharacterFeatureOrTrait5e>();
            Proficiencies = new List<CharacterProficiency5e>();
            AlliesAndOrganizations = new List<CharacterAllyOrOrganization5e>();
            Spells = new List<CharacterSpell5e>();
        }

        public Guid Key { get; set; }

        public string Name { get; set; }

        public string Class { get; set; }

        public int Level { get; set; }

        public string Background { get; set; }

        public string PlayerName { get; set; }

        public string Race { get; set; }

        public string Alignment { get; set; }

        public int Experience { get; set; }

        public int Strength { get; set; }

        public int Dexterity { get; set; }

        public int Constitution { get; set; }

        public int Intelligence { get; set; }

        public int Wisdom { get; set; }

        public int Charisma { get; set; }

        public int Inspiration { get; set; }

        public int ProficiencyBonus { get; set; }

        public CharacterSavingThrows5e SavingThrows { get; set; }

        public CharacterSkills5e Skills { get; set; }

        public int ArmorClass { get; set; }

        public int Initiative { get; set; }

        public int Speed { get; set; }

        public int HitPointMaximum { get; set; }

        public int CurrentHitPoints { get; set; }

        public int? TemporaryHitPoints { get; set; }

        public string TotalHitDice { get; set; }

        public string HitDice { get; set; }

        public int SuccessDeathSaves { get; set; }

        public int FailedDeathSaves { get; set; }

        public IList<CharacterAttackOrSpellcast5e> AttacksAndSpellcasts { get; set; }

        public IList<CharacterItem5e> Items { get; set; }

        public CharacterMoney5e Money { get; set; }

        public string PersonalityTraits { get; set; }

        public string Ideals { get; set; }

        public string Bonds { get; set; }

        public string Flaws { get; set; }

        public IList<CharacterFeatureOrTrait5e> FeaturesAndTraits { get; set; }

        public int PassiveWisdom { get; set; }

        public IList<CharacterProficiency5e> Proficiencies { get; set; }

        public IList<CharacterAllyOrOrganization5e> AlliesAndOrganizations { get; set; }

        public int Age { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string Eyes { get; set; }

        public string Skin { get; set; }

        public string Hair { get; set; }

        public string SpellcastingClass { get; set; }

        public int? SpellcastingAbility { get; set; }

        public int? SpellSaveDC { get; set; }

        public int? SpellAttackBonus { get; set; }

        public IList<CharacterSpell5e> Spells { get; set; }

        public CharacterSpellSlots5e SpellSlots { get; set; }

        public Guid? AppearanceKey { get; set; }

        public string Backstory { get; set; }
    }
}
