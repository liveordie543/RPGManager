using RPGManager.Data.Models;
using System;

namespace RPGManager.Data.Stores
{
    public interface ICharacterStore
    {
        ICharacter GetCharacterByKey(Guid key);

        bool SaveCharacter(ICharacter character);

        bool DeleteCharacter(Guid key);
    }
}
