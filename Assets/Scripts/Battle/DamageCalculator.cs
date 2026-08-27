using UnityEngine;

public static class DamageCalculator
{
    private const float CriticalChance = 0.0625f;
    private const float CriticalMultiplier = 1.5f;

    public static int CalculateDamage(Pet attacker, Pet defender, Move move)
    {
        if (move.power <= 0)
        {
            return 0;
        }

        bool physical = move.category == MoveCategory.Physical;
        int attack = attacker.GetStat(physical ? StatType.Attack : StatType.SpAttack);
        int defense = defender.GetStat(physical ? StatType.Defense : StatType.SpDefense);

        float stab = move.type == attacker.species.primaryType || move.type == attacker.species.secondaryType ? 1.5f : 1f;
        float typeEffectiveness = TypeChart.GetEffectiveness(move.type, defender.species.primaryType, defender.species.secondaryType);
        float critical = Random.value < CriticalChance ? CriticalMultiplier : 1f;
        float randomFactor = Random.Range(0.85f, 1f);

        float baseDamage = (2f * attack / defense) * (move.power / 50f) + 2f;
        float total = baseDamage * stab * typeEffectiveness * critical * randomFactor;

        return Mathf.Max(1, Mathf.FloorToInt(total));
    }
}
