using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigUtils
{
    public static string GetCharacterName(int characterId)
    {
        return m_tables.TbCharacter.Get(characterId).Name;
    }
}
