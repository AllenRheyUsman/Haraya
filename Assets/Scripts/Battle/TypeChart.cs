using System.Collections.Generic;

public static class TypeChart
{
    // TODO: fill in the full 18x18 effectiveness matrix. Only a few illustrative
    // entries are seeded below; anything not listed defaults to neutral (1x).
    private static readonly Dictionary<(ElementType attack, ElementType defend), float> effectiveness = new()
    {
        { (ElementType.Fire, ElementType.Grass), 2f },
        { (ElementType.Fire, ElementType.Water), 0.5f },
        { (ElementType.Water, ElementType.Fire), 2f },
        { (ElementType.Water, ElementType.Grass), 0.5f },
        { (ElementType.Grass, ElementType.Water), 2f },
        { (ElementType.Grass, ElementType.Fire), 0.5f },
        { (ElementType.Electric, ElementType.Water), 2f },
        { (ElementType.Electric, ElementType.Ground), 0f },
    };

    public static float GetEffectiveness(ElementType attack, ElementType defend)
    {
        return effectiveness.GetValueOrDefault((attack, defend), 1f);
    }

    public static float GetEffectiveness(ElementType attack, ElementType defendPrimary, ElementType defendSecondary)
    {
        float total = GetEffectiveness(attack, defendPrimary);
        if (defendSecondary != ElementType.None)
        {
            total *= GetEffectiveness(attack, defendSecondary);
        }

        return total;
    }
}
