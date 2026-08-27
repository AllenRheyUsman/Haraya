using System;
using System.Collections.Generic;

[Serializable]
public class Pet
{
    public string instanceId;
    public PetSpecies species;
    public string nickname;
    public int level = 1;
    public int experience;
    public Nature nature;

    public Dictionary<StatType, int> ivs = new();
    public Dictionary<StatType, int> evs = new();
    public List<Move> knownMoves = new(4);

    public int currentHP;
    public StatusCondition status = StatusCondition.None;
    public int happiness = 70;

    // Care System (0-100; hunger/hygiene tick up over time, energy ticks down)
    public float hunger;
    public float energy = 100f;
    public float hygiene = 100f;

    public bool IsFainted => currentHP <= 0;

    public int GetStat(StatType stat)
    {
        int iv = ivs.GetValueOrDefault(stat, 0);
        int ev = evs.GetValueOrDefault(stat, 0);
        return PetStats.CalculateStat(stat, species.GetBaseStat(stat), iv, ev, level, nature);
    }

    public void GainExperience(int amount)
    {
        // TODO: level curve, level-up stat recalculation, movepool learn checks, evolution checks.
        experience += amount;
    }

    public bool CanEvolve()
    {
        return !string.IsNullOrEmpty(species.evolvesIntoId) && level >= species.evolutionLevel;
    }
}
