using System.Collections.Generic;

public enum Nature
{
    Hardy, Lonely, Brave, Adamant, Naughty,
    Bold, Docile, Relaxed, Impish, Lax,
    Timid, Hasty, Serious, Jolly, Naive,
    Modest, Mild, Quiet, Bashful, Rash,
    Calm, Gentle, Sassy, Careful, Quirky
}

public static class NatureModifiers
{
    // Neutral natures (Hardy, Docile, Serious, Bashful, Quirky) boost/reduce the same stat, net +/-0%.
    private static readonly Dictionary<Nature, (StatType boosted, StatType reduced)> modifiers = new()
    {
        { Nature.Lonely, (StatType.Attack, StatType.Defense) },
        { Nature.Brave, (StatType.Attack, StatType.Speed) },
        { Nature.Adamant, (StatType.Attack, StatType.SpAttack) },
        { Nature.Naughty, (StatType.Attack, StatType.SpDefense) },
        { Nature.Bold, (StatType.Defense, StatType.Attack) },
        { Nature.Relaxed, (StatType.Defense, StatType.Speed) },
        { Nature.Impish, (StatType.Defense, StatType.SpAttack) },
        { Nature.Lax, (StatType.Defense, StatType.SpDefense) },
        { Nature.Timid, (StatType.Speed, StatType.Attack) },
        { Nature.Hasty, (StatType.Speed, StatType.Defense) },
        { Nature.Jolly, (StatType.Speed, StatType.SpAttack) },
        { Nature.Naive, (StatType.Speed, StatType.SpDefense) },
        { Nature.Modest, (StatType.SpAttack, StatType.Attack) },
        { Nature.Mild, (StatType.SpAttack, StatType.Defense) },
        { Nature.Quiet, (StatType.SpAttack, StatType.Speed) },
        { Nature.Rash, (StatType.SpAttack, StatType.SpDefense) },
        { Nature.Calm, (StatType.SpDefense, StatType.Attack) },
        { Nature.Gentle, (StatType.SpDefense, StatType.Defense) },
        { Nature.Sassy, (StatType.SpDefense, StatType.Speed) },
        { Nature.Careful, (StatType.SpDefense, StatType.SpAttack) },
    };

    public static float GetMultiplier(Nature nature, StatType stat)
    {
        if (!modifiers.TryGetValue(nature, out var mod))
        {
            return 1f;
        }

        if (mod.boosted == stat)
        {
            return 1.1f;
        }

        if (mod.reduced == stat)
        {
            return 0.9f;
        }

        return 1f;
    }
}
