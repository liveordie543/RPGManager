namespace RPGManager.Data.Models
{
    public class CharacterAttackOrSpellcast5e
    {
        public CharacterAttackOrSpellcast5e()
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int AttackBonus { get; set; }

        public string Damage { get; set; }

        public string DamageType { get; set; }

        public string Notes { get; set; }
    }
}
