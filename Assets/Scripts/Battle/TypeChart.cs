using System.Collections.Generic;

public static class TypeChart
{
    private static readonly Dictionary<(ElementType attack, ElementType defend), float> effectiveness = new()
    {
        // Normal
        { (ElementType.Normal, ElementType.Rock), 0.5f }, { (ElementType.Normal, ElementType.Ghost), 0f }, { (ElementType.Normal, ElementType.Steel), 0.5f },
        // Fire
        { (ElementType.Fire, ElementType.Grass), 2f }, { (ElementType.Fire, ElementType.Ice), 2f }, { (ElementType.Fire, ElementType.Bug), 2f }, { (ElementType.Fire, ElementType.Steel), 2f },
        { (ElementType.Fire, ElementType.Fire), 0.5f }, { (ElementType.Fire, ElementType.Water), 0.5f }, { (ElementType.Fire, ElementType.Rock), 0.5f }, { (ElementType.Fire, ElementType.Dragon), 0.5f },
        // Water
        { (ElementType.Water, ElementType.Fire), 2f }, { (ElementType.Water, ElementType.Ground), 2f }, { (ElementType.Water, ElementType.Rock), 2f },
        { (ElementType.Water, ElementType.Water), 0.5f }, { (ElementType.Water, ElementType.Grass), 0.5f }, { (ElementType.Water, ElementType.Dragon), 0.5f },
        // Electric
        { (ElementType.Electric, ElementType.Water), 2f }, { (ElementType.Electric, ElementType.Flying), 2f },
        { (ElementType.Electric, ElementType.Electric), 0.5f }, { (ElementType.Electric, ElementType.Grass), 0.5f }, { (ElementType.Electric, ElementType.Dragon), 0.5f }, { (ElementType.Electric, ElementType.Ground), 0f },
        // Grass
        { (ElementType.Grass, ElementType.Water), 2f }, { (ElementType.Grass, ElementType.Ground), 2f }, { (ElementType.Grass, ElementType.Rock), 2f },
        { (ElementType.Grass, ElementType.Fire), 0.5f }, { (ElementType.Grass, ElementType.Grass), 0.5f }, { (ElementType.Grass, ElementType.Poison), 0.5f },
        { (ElementType.Grass, ElementType.Flying), 0.5f }, { (ElementType.Grass, ElementType.Bug), 0.5f }, { (ElementType.Grass, ElementType.Dragon), 0.5f }, { (ElementType.Grass, ElementType.Steel), 0.5f },
        // Ice
        { (ElementType.Ice, ElementType.Grass), 2f }, { (ElementType.Ice, ElementType.Ground), 2f }, { (ElementType.Ice, ElementType.Flying), 2f }, { (ElementType.Ice, ElementType.Dragon), 2f },
        { (ElementType.Ice, ElementType.Fire), 0.5f }, { (ElementType.Ice, ElementType.Water), 0.5f }, { (ElementType.Ice, ElementType.Ice), 0.5f }, { (ElementType.Ice, ElementType.Steel), 0.5f },
        // Fighting
        { (ElementType.Fighting, ElementType.Normal), 2f }, { (ElementType.Fighting, ElementType.Ice), 2f }, { (ElementType.Fighting, ElementType.Rock), 2f },
        { (ElementType.Fighting, ElementType.Dark), 2f }, { (ElementType.Fighting, ElementType.Steel), 2f },
        { (ElementType.Fighting, ElementType.Poison), 0.5f }, { (ElementType.Fighting, ElementType.Flying), 0.5f }, { (ElementType.Fighting, ElementType.Psychic), 0.5f },
        { (ElementType.Fighting, ElementType.Bug), 0.5f }, { (ElementType.Fighting, ElementType.Fairy), 0.5f }, { (ElementType.Fighting, ElementType.Ghost), 0f },
        // Poison
        { (ElementType.Poison, ElementType.Grass), 2f }, { (ElementType.Poison, ElementType.Fairy), 2f },
        { (ElementType.Poison, ElementType.Poison), 0.5f }, { (ElementType.Poison, ElementType.Ground), 0.5f }, { (ElementType.Poison, ElementType.Rock), 0.5f }, { (ElementType.Poison, ElementType.Ghost), 0.5f }, { (ElementType.Poison, ElementType.Steel), 0f },
        // Ground
        { (ElementType.Ground, ElementType.Fire), 2f }, { (ElementType.Ground, ElementType.Electric), 2f }, { (ElementType.Ground, ElementType.Poison), 2f },
        { (ElementType.Ground, ElementType.Rock), 2f }, { (ElementType.Ground, ElementType.Steel), 2f },
        { (ElementType.Ground, ElementType.Grass), 0.5f }, { (ElementType.Ground, ElementType.Bug), 0.5f }, { (ElementType.Ground, ElementType.Flying), 0f },
        // Flying
        { (ElementType.Flying, ElementType.Grass), 2f }, { (ElementType.Flying, ElementType.Fighting), 2f }, { (ElementType.Flying, ElementType.Bug), 2f },
        { (ElementType.Flying, ElementType.Electric), 0.5f }, { (ElementType.Flying, ElementType.Rock), 0.5f }, { (ElementType.Flying, ElementType.Steel), 0.5f },
        // Psychic
        { (ElementType.Psychic, ElementType.Fighting), 2f }, { (ElementType.Psychic, ElementType.Poison), 2f },
        { (ElementType.Psychic, ElementType.Psychic), 0.5f }, { (ElementType.Psychic, ElementType.Steel), 0.5f }, { (ElementType.Psychic, ElementType.Dark), 0f },
        // Bug
        { (ElementType.Bug, ElementType.Grass), 2f }, { (ElementType.Bug, ElementType.Psychic), 2f }, { (ElementType.Bug, ElementType.Dark), 2f },
        { (ElementType.Bug, ElementType.Fire), 0.5f }, { (ElementType.Bug, ElementType.Fighting), 0.5f }, { (ElementType.Bug, ElementType.Poison), 0.5f },
        { (ElementType.Bug, ElementType.Flying), 0.5f }, { (ElementType.Bug, ElementType.Ghost), 0.5f }, { (ElementType.Bug, ElementType.Steel), 0.5f }, { (ElementType.Bug, ElementType.Fairy), 0.5f },
        // Rock
        { (ElementType.Rock, ElementType.Fire), 2f }, { (ElementType.Rock, ElementType.Ice), 2f }, { (ElementType.Rock, ElementType.Flying), 2f }, { (ElementType.Rock, ElementType.Bug), 2f },
        { (ElementType.Rock, ElementType.Fighting), 0.5f }, { (ElementType.Rock, ElementType.Ground), 0.5f }, { (ElementType.Rock, ElementType.Steel), 0.5f },
        // Ghost
        { (ElementType.Ghost, ElementType.Ghost), 2f }, { (ElementType.Ghost, ElementType.Psychic), 2f },
        { (ElementType.Ghost, ElementType.Dark), 0.5f }, { (ElementType.Ghost, ElementType.Normal), 0f },
        // Dragon
        { (ElementType.Dragon, ElementType.Dragon), 2f }, { (ElementType.Dragon, ElementType.Steel), 0.5f }, { (ElementType.Dragon, ElementType.Fairy), 0f },
        // Dark
        { (ElementType.Dark, ElementType.Psychic), 2f }, { (ElementType.Dark, ElementType.Ghost), 2f },
        { (ElementType.Dark, ElementType.Fighting), 0.5f }, { (ElementType.Dark, ElementType.Dark), 0.5f }, { (ElementType.Dark, ElementType.Fairy), 0.5f },
        // Steel
        { (ElementType.Steel, ElementType.Ice), 2f }, { (ElementType.Steel, ElementType.Rock), 2f }, { (ElementType.Steel, ElementType.Fairy), 2f },
        { (ElementType.Steel, ElementType.Fire), 0.5f }, { (ElementType.Steel, ElementType.Water), 0.5f }, { (ElementType.Steel, ElementType.Electric), 0.5f }, { (ElementType.Steel, ElementType.Steel), 0.5f },
        // Fairy
        { (ElementType.Fairy, ElementType.Fighting), 2f }, { (ElementType.Fairy, ElementType.Dragon), 2f }, { (ElementType.Fairy, ElementType.Dark), 2f },
        { (ElementType.Fairy, ElementType.Fire), 0.5f }, { (ElementType.Fairy, ElementType.Poison), 0.5f }, { (ElementType.Fairy, ElementType.Steel), 0.5f },
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
