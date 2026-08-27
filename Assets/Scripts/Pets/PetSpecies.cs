using System;
using System.Collections.Generic;

[Serializable]
public class LevelUpMove
{
    public int level;
    public Move move;
}

[Serializable]
public class PetSpecies
{
    public string speciesId;
    public string speciesName;
    public string modelPath;
    public ElementType primaryType;
    public ElementType secondaryType = ElementType.None;

    public int baseHP;
    public int baseAttack;
    public int baseDefense;
    public int baseSpAttack;
    public int baseSpDefense;
    public int baseSpeed;

    public List<LevelUpMove> movepool = new();

    public string evolvesIntoId;
    public int evolutionLevel;

    public int GetBaseStat(StatType stat)
    {
        return stat switch
        {
            StatType.HP => baseHP,
            StatType.Attack => baseAttack,
            StatType.Defense => baseDefense,
            StatType.SpAttack => baseSpAttack,
            StatType.SpDefense => baseSpDefense,
            StatType.Speed => baseSpeed,
            _ => 0
        };
    }
}
