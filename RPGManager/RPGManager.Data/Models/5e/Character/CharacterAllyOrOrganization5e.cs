using System;

namespace RPGManager.Data.Models
{
    public class CharacterAllyOrOrganization5e
    {
        public CharacterAllyOrOrganization5e()
        {
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ImageKey { get; set; }
    }
}
