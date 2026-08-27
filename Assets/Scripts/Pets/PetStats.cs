using UnityEngine;

public static class PetStats
{
    public static int CalculateStat(StatType stat, int baseStat, int iv, int ev, int level, Nature nature)
    {
        int raw = Mathf.FloorToInt((2 * baseStat + iv + ev / 4) * level / 100f);

        if (stat == StatType.HP)
        {
            return raw + level + 10;
        }

        float withNature = (raw + 5) * NatureModifiers.GetMultiplier(nature, stat);
        return Mathf.FloorToInt(withNature);
    }
}
