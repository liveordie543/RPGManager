using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGManager.Data.Models;
using System.Data;
using System.Data.SqlClient;
using RPGManager.Extensions;

namespace RPGManager.Data.Stores
{
    public class CharacterStore5e : ICharacterStore
    {
        protected string _connectionString;

        public CharacterStore5e()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public ICharacter GetCharacterByKey(Guid key)
        {
            Character5e character = null;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Key", key)
            };

            using (CustomDataAdapter adapter = new CustomDataAdapter("GetCharacter5eByKey", CommandType.StoredProcedure, parameters))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                if(table.Rows.Count > 0)
                {
                    character = CreateCharacterObject(table.Rows[0]);
                }
            }

            return character;
        }

        public bool SaveCharacter(ICharacter character)
        {
            if (!(character is Character5e))
            {
                return false;
            }

            bool result = false;
            Character5e character5e = character as Character5e;

            //using (CustomDataAdapter adapter = new CustomDataAdapter("AddOrUpdateCharacter5e", CommandType.StoredProcedure, parameters))
            //{
            //    result = adapter.ExecuteNonQuery() == 1;
            //}

            SqlTransaction transaction = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    transaction = connection.BeginTransaction();

                    using (SqlCommand command = new SqlCommand("AddOrUpdateCharacter5e", connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddRange(character5e.ToSqlParameters().ToArray());
                        command.Parameters.AddRange(character5e.Skills.ToSqlParameters().ToArray());
                        command.Parameters.AddRange(character5e.Money.ToSqlParameters().ToArray());
                        command.Parameters.AddRange(character5e.SpellSlots.ToSqlParameters().ToArray());

                        command.Parameters.AddRange(new SqlParameter[]
                        {
                            new SqlParameter("@CstStrength", character5e.SavingThrows.Strength),
                            new SqlParameter("@CstDexterity", character5e.SavingThrows.Dexterity),
                            new SqlParameter("@CstConstitution", character5e.SavingThrows.Constitution),
                            new SqlParameter("@CstIntelligence", character5e.SavingThrows.Intelligence),
                            new SqlParameter("@CstWisdom", character5e.SavingThrows.Wisdom),
                            new SqlParameter("@CstCharisma", character5e.SavingThrows.Charisma),
                        });

                        command.ExecuteNonQuery();
                    }

                    ExecuteCmdAgainstEntityList(character5e.AttacksAndSpellcasts, "AddOrUpdateCharacterAttacksAndSpellcasts5e", connection, transaction);
                    ExecuteCmdAgainstEntityList(character5e.Items, "AddOrUpdateCharacterItem5e", connection, transaction);
                    ExecuteCmdAgainstEntityList(character5e.FeaturesAndTraits, "AddOrUpdateCharacterFeatureOrTrait5e", connection, transaction);
                    ExecuteCmdAgainstEntityList(character5e.Proficiencies, "AddOrUpdateCharacterProficiency5e", connection, transaction);
                    ExecuteCmdAgainstEntityList(character5e.AlliesAndOrganizations, "AddOrUpdateCharacterAllyOrOrganization5e", connection, transaction);
                    ExecuteCmdAgainstEntityList(character5e.Spells, "AddOrUpdateCharacterCharacterSpell5e", connection, transaction);

                    transaction.Commit();
                }         
            }
            catch (Exception)
            {
                if(transaction != null)
                {
                    transaction.Rollback();
                }

                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }

            return true;
        }

        public bool DeleteCharacter(Guid key)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Key", key)
            };

            using (CustomDataAdapter adapter = new CustomDataAdapter("DeleteCharacter5eByKey", CommandType.StoredProcedure, parameters))
            {
                adapter.ExecuteNonQuery();
            }

            return true;
        }

        private Character5e CreateCharacterObject(DataRow row)
        {
            int characterId = Convert.ToInt32(row["Id"]);

            Character5e character = DataRowHelper.CreateItemFromRow<Character5e>(row);

            character.Key = (Guid)row["Key"];
            character.SavingThrows = CreateSavingThrowsObject(row);
            character.Skills = DataRowHelper.CreateItemFromRow<CharacterSkills5e>(row); //CreateSkillsObject(row);
            character.TemporaryHitPoints = row["TemporaryHitPoints"].ToNullableInt();
            character.AttacksAndSpellcasts = GetAttacksAndSpellcasts(characterId);
            character.Items = GetItems(characterId);
            character.Money = DataRowHelper.CreateItemFromRow<CharacterMoney5e>(row); //CreateMoneyObject(row);
            character.FeaturesAndTraits = GetFeaturesAndTraits(characterId);
            character.Proficiencies = GetProficiencies(characterId);
            character.AlliesAndOrganizations = GetAlliesAndOrganizations(characterId);
            character.SpellcastingAbility = row["SpellcastingAbility"].ToNullableInt();
            character.SpellSaveDC = row["SpellSaveDC"].ToNullableInt();
            character.SpellAttackBonus = row["SpellAttackBonus"].ToNullableInt();
            character.Spells = GetSpells(characterId);
            character.SpellSlots = DataRowHelper.CreateItemFromRow<CharacterSpellSlots5e>(row); //CreateSpellSlotsObject(row);
            character.AppearanceKey = (Guid)row["AppearanceKey"];

            return character;

            //return new Character5e
            //{
            //    Key = (Guid)row["Key"],
            //    Name = row["Name"] as String,
            //    Class = row["Class"] as String,
            //    Level = Convert.ToInt32(row["Name"]),
            //    Background = row["Background"] as String,
            //    PlayerName = row["PlayerName"] as String,
            //    Race = row["Race"] as String,
            //    Alignment = row["Alignment"] as String,
            //    Experience = Convert.ToInt32(row["Experience"]),
            //    Strength = Convert.ToInt32(row["Strength"]),
            //    Dexterity = Convert.ToInt32(row["Dexterity"]),
            //    Constitution = Convert.ToInt32(row["Constitution"]),
            //    Intelligence = Convert.ToInt32(row["Intelligence"]),
            //    Wisdom = Convert.ToInt32(row["Wisdom"]),
            //    Charisma = Convert.ToInt32(row["Charisma"]),
            //    Inspiration = Convert.ToInt32(row["Inspiration"]),
            //    ProficiencyBonus = Convert.ToInt32(row["ProficiencyBonus"]),
            //    SavingThrows = CreateSavingThrowsObject(row),
            //    Skills = CreateSkillsObject(row),
            //    ArmorClass = Convert.ToInt32(row["ArmorClass"]),
            //    Initiative = Convert.ToInt32(row["Initiative"]),
            //    Speed = Convert.ToInt32(row["Speed"]),
            //    HitPointMaximum = Convert.ToInt32(row["HitPointMaximum"]),
            //    CurrentHitPoints = Convert.ToInt32(row["CurrentHitPoints"]),
            //    TemporaryHitPoints = row["TemporaryHitPoints"].ToNullableInt(),
            //    TotalHitDice = row["TotalHitDice"] as String,
            //    HitDice = row["HitDice"] as String,
            //    SuccessDeathSaves = Convert.ToInt32(row["SuccessDeathSaves"]),
            //    FailedDeathSaves = Convert.ToInt32(row["FailedDeathSaves"]),
            //    AttacksAndSpellcasts = GetAttacksAndSpellcasts(characterId),
            //    //Items = row["Items"] as String,
            //    Money = CreateMoneyObject(row),
            //    PersonalityTraits = row["PersonalityTraits"] as String,
            //    Ideals = row["Ideals"] as String,
            //    Bonds = row["Bonds"] as String,
            //    Flaws = row["Flaws"] as String,
            //    //FeaturesAndTraits = row["FeaturesAndTraits"] as String,
            //    PassiveWisdom = Convert.ToInt32(row["PassiveWisdom"]),
            //    //Proficiencies = row["Proficiencies"] as String,
            //    //AlliesAndOrganizations = row["AlliesAndOrganizations"] as String,
            //    Age = Convert.ToInt32(row["Age"]),
            //    Height = row["Height"] as String,
            //    Weight = row["Weight"] as String,
            //    Eyes = row["Eyes"] as String,
            //    Skin = row["Skin"] as String,
            //    Hair = row["Hair"] as String,
            //    SpellcastingClass = row["SpellcastingClass"] as String,
            //    SpellcastingAbility = row["SpellcastingAbility"].ToNullableInt(),
            //    SpellSaveDC = row["SpellSaveDC"].ToNullableInt(),
            //    SpellAttackBonus = row["SpellAttackBonus"].ToNullableInt(),
            //    //Spells = row["Spells"] as String,
            //    SpellSlots = CreateSpellSlotsObject(row),
            //    AppearanceKey = (Guid)row["AppearanceKey"],
            //    Backstory = row["Backstory"] as String
            //};
        }

        private IList<CharacterAttackOrSpellcast5e> GetAttacksAndSpellcasts(int characterId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CharacterId", characterId)
            };

            using (CustomDataAdapter adapter = new CustomDataAdapter("GetCharacterAttacksAndSpellcasts5e", CommandType.StoredProcedure, parameters))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return DataRowHelper.CreateListFromTable<CharacterAttackOrSpellcast5e>(table);

                //foreach(DataRow row in table.Rows)
                //{
                //    attacksAndSpellcasts.Add(new CharacterAttackOrSpellcast5e
                //    {
                //        Name = row["Name"] as String,
                //        AttackBonus = Convert.ToInt32(row["AttackBonus"]),
                //        Damage = row["Damage"] as String,
                //        DamageType = row["DamageType"] as String,
                //        Notes = row["Notes"] as String,
                //    });
                //}
            }
        }

        private IList<CharacterItem5e> GetItems(int characterId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CharacterId", characterId)
            };

            using (CustomDataAdapter adapter = new CustomDataAdapter("GetCharacterItems5e", CommandType.StoredProcedure, parameters))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return DataRowHelper.CreateListFromTable<CharacterItem5e>(table);
            }
        }

        private IList<CharacterFeatureOrTrait5e> GetFeaturesAndTraits(int characterId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CharacterId", characterId)
            };

            using (CustomDataAdapter adapter = new CustomDataAdapter("GetCharacterFeaturesAndTraits5e", CommandType.StoredProcedure, parameters))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return DataRowHelper.CreateListFromTable<CharacterFeatureOrTrait5e>(table);
            }
        }

        private IList<CharacterProficiency5e> GetProficiencies(int characterId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CharacterId", characterId)
            };

            using (CustomDataAdapter adapter = new CustomDataAdapter("GetCharacterProficiencies5e", CommandType.StoredProcedure, parameters))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return DataRowHelper.CreateListFromTable<CharacterProficiency5e>(table);
            }
        }

        private IList<CharacterAllyOrOrganization5e> GetAlliesAndOrganizations(int characterId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CharacterId", characterId)
            };

            using (CustomDataAdapter adapter = new CustomDataAdapter("GetCharacterAlliesAndOrganizations5e", CommandType.StoredProcedure, parameters))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                List<CharacterAllyOrOrganization5e> alliesAndOrganizations = new List<CharacterAllyOrOrganization5e>();

                foreach(DataRow row in table.Rows)
                {
                    CharacterAllyOrOrganization5e allyOrOrganization = DataRowHelper.CreateItemFromRow<CharacterAllyOrOrganization5e>(row);
                    allyOrOrganization.ImageKey = row["ImageKey"].ToNullableGuid();

                    alliesAndOrganizations.Add(allyOrOrganization);
                }

                return alliesAndOrganizations;
            }
        }

        private IList<CharacterSpell5e> GetSpells(int characterId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CharacterId", characterId)
            };

            using (CustomDataAdapter adapter = new CustomDataAdapter("GetCharacterSpells5e", CommandType.StoredProcedure, parameters))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return DataRowHelper.CreateListFromTable<CharacterSpell5e>(table);
            }
        }

        private CharacterSavingThrows5e CreateSavingThrowsObject(DataRow row)
        {
            return new CharacterSavingThrows5e
            {
                Strength = Convert.ToInt32(row["cstStrength"]),
                Dexterity = Convert.ToInt32(row["cstDexterity"]),
                Constitution = Convert.ToInt32(row["cstConstitution"]),
                Intelligence = Convert.ToInt32(row["cstIntelligence"]),
                Wisdom = Convert.ToInt32(row["cstWisdom"]),
                Charisma = Convert.ToInt32(row["cstCharisma"])
            };
        }

        private void ExecuteCmdAgainstEntityList<T>(IList<T> entities, string commandName, SqlConnection connection, SqlTransaction transaction)
        {
            foreach (T entity in entities)
            {
                if(!(entity is DbEntity))
                {
                    return;
                }

                DbEntity baseEntity = entity as DbEntity;

                if (baseEntity.IsChanged)
                {
                    using (SqlCommand command = new SqlCommand(commandName, connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddRange(entity.ToSqlParameters().ToArray());

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        //private CharacterSkills5e CreateSkillsObject(DataRow row)
        //{
        //    return new CharacterSkills5e
        //    {
        //        Acrobatics = Convert.ToInt32(row["Acrobatics"]),
        //        AcrobaticsIsProficient = Convert.ToBoolean(row["AcrobaticsIsProficient"]),
        //        AnimalHandling = Convert.ToInt32(row["AnimalHandling"]),
        //        AnimalHandlingIsProficient = Convert.ToBoolean(row["AnimalHandlingIsProficient"]),
        //        Arcana = Convert.ToInt32(row["Arcana"]),
        //        ArcanaIsProficient = Convert.ToBoolean(row["ArcanaIsProficient"]),
        //        Athletics = Convert.ToInt32(row["Athletics"]),
        //        AthleticsIsProficient = Convert.ToBoolean(row["AthleticsIsProficient"]),
        //        Deception = Convert.ToInt32(row["Deception"]),
        //        DeceptionIsProficient = Convert.ToBoolean(row["DeceptionIsProficient"]),
        //        History = Convert.ToInt32(row["History"]),
        //        HistoryIsProficient = Convert.ToBoolean(row["HistoryIsProficient"]),
        //        Insight = Convert.ToInt32(row["Insight"]),
        //        InsightIsProficient = Convert.ToBoolean(row["InsightIsProficient"]),
        //        Intimidation = Convert.ToInt32(row["Intimidation"]),
        //        IntimidationIsProficient = Convert.ToBoolean(row["IntimidationIsProficient"]),
        //        Investigation = Convert.ToInt32(row["Investigation"]),
        //        InvestigationIsProficient = Convert.ToBoolean(row["InvestigationIsProficient"]),
        //        Medicine = Convert.ToInt32(row["Medicine"]),
        //        MedicineIsProficient = Convert.ToBoolean(row["MedicineIsProficient"]),
        //        Nature = Convert.ToInt32(row["Nature"]),
        //        NatureIsProficient = Convert.ToBoolean(row["NatureIsProficient"]),
        //        Perception = Convert.ToInt32(row["Perception"]),
        //        PerceptionIsProficient = Convert.ToBoolean(row["PerceptionIsProficient"]),
        //        Performance = Convert.ToInt32(row["Performance"]),
        //        PerformanceIsProficient = Convert.ToBoolean(row["PerformanceIsProficient"]),
        //        Persuasion = Convert.ToInt32(row["Persuasion"]),
        //        PersuasionIsProficient = Convert.ToBoolean(row["PersuasionIsProficient"]),
        //        Religion = Convert.ToInt32(row["Religion"]),
        //        ReligionIsProficient = Convert.ToBoolean(row["ReligionIsProficient"]),
        //        SleightOfHand = Convert.ToInt32(row["SleightOfHand"]),
        //        SleightOfHandIsProficient = Convert.ToBoolean(row["SleightOfHandIsProficient"]),
        //        Stealth = Convert.ToInt32(row["Stealth"]),
        //        StealthIsProficient = Convert.ToBoolean(row["StealthIsProficient"]),
        //        Survival = Convert.ToInt32(row["Survival"]),
        //        SurvivalIsProficient = Convert.ToBoolean(row["SurvivalIsProficient"])
        //    };
        //}

        //private CharacterMoney5e CreateMoneyObject(DataRow row)
        //{
        //    return new CharacterMoney5e
        //    {
        //        CP = Convert.ToInt32(row["CP"]),
        //        SP = Convert.ToInt32(row["SP"]),
        //        EP = Convert.ToInt32(row["EP"]),
        //        GP = Convert.ToInt32(row["GP"]),
        //        PP = Convert.ToInt32(row["PP"])
        //    };
        //}

        //private CharacterSpellSlots5e CreateSpellSlotsObject(DataRow row)
        //{
        //    return new CharacterSpellSlots5e
        //    {
        //        Level0Max = Convert.ToInt32(row["Level0Max"]),
        //        Level0Used = Convert.ToInt32(row["Level0Used"]),
        //        Level1Max = Convert.ToInt32(row["Level1Max"]),
        //        Level1Used = Convert.ToInt32(row["Level1Used"]),
        //        Level2Max = Convert.ToInt32(row["Level2Max"]),
        //        Level2Used = Convert.ToInt32(row["Level2Used"]),
        //        Level3Max = Convert.ToInt32(row["Level3Max"]),
        //        Level3Used = Convert.ToInt32(row["Level3Used"]),
        //        Level4Max = Convert.ToInt32(row["Level4Max"]),
        //        Level4Used = Convert.ToInt32(row["Level4Used"]),
        //        Level5Max = Convert.ToInt32(row["Level5Max"]),
        //        Level5Used = Convert.ToInt32(row["Level5Used"]),
        //        Level6Max = Convert.ToInt32(row["Level6Max"]),
        //        Level6Used = Convert.ToInt32(row["Level6Used"]),
        //        Level7Max = Convert.ToInt32(row["Level7Max"]),
        //        Level7Used = Convert.ToInt32(row["Level7Used"]),
        //        Level8Max = Convert.ToInt32(row["Level8Max"]),
        //        Level8Used = Convert.ToInt32(row["Level8Used"]),
        //        Level9Max = Convert.ToInt32(row["Level9Max"]),
        //        Level9Used = Convert.ToInt32(row["Level9Used"])
        //    };
        //}
    }
}
