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

    // Populates knownMoves for a pet created directly at its current level (spawned wild
    // encounters, starter pets) - LevelUp() only fires for levels actually crossed via
    // GainExperience, so a freshly-created pet needs this instead. Keeps the 4 highest-level
    // applicable moves, since that's what the pet would know if it had leveled up normally.
    public void InitializeStartingMoves()
    {
        knownMoves.Clear();
        var applicable = new List<Move>();
        foreach (var entry in species.movepool)
        {
            if (entry.level <= level)
            {
                applicable.Add(entry.move);
            }
        }

        int start = Math.Max(0, applicable.Count - 4);
        for (int i = start; i < applicable.Count; i++)
        {
            knownMoves.Add(applicable[i].Clone());
        }
    }

    // Medium Fast growth curve (matches the classic Pokemon formula): level^3 total XP to reach that level.
    public static int ExperienceForLevel(int lvl) => lvl * lvl * lvl;

    public void GainExperience(int amount)
    {
        experience += amount;

        while (ExperienceForLevel(level + 1) <= experience)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        int oldMaxHP = GetStat(StatType.HP);
        level++;

        // Evolve BEFORE the movepool check: an evolved species' move at the evolution level
        // (e.g. Tomcat's "Slash" at Lv.8) should be learned immediately, not on the next level.
        if (CanEvolve())
        {
            var evolvedSpecies = GameDataLoader.GetSpecies(species.evolvesIntoId);
            if (evolvedSpecies != null)
            {
                species = evolvedSpecies;
            }
        }

        int newMaxHP = GetStat(StatType.HP);
        currentHP += newMaxHP - oldMaxHP;

        foreach (var entry in species.movepool)
        {
            if (entry.level == level)
            {
                LearnMove(entry.move);
            }
        }
    }

    private void LearnMove(Move move)
    {
        if (knownMoves.Count >= 4)
        {
            // TODO: real games prompt the player to choose which move to forget; for now the new move is skipped.
            return;
        }

        knownMoves.Add(move.Clone());
    }

    public bool CanEvolve()
    {
        return !string.IsNullOrEmpty(species.evolvesIntoId) && level >= species.evolutionLevel;
    }

    public bool TryEvolve()
    {
        if (!CanEvolve())
        {
            return false;
        }

        var evolvedSpecies = GameDataLoader.GetSpecies(species.evolvesIntoId);
        if (evolvedSpecies == null)
        {
            return false;
        }

        int oldMaxHP = GetStat(StatType.HP);
        species = evolvedSpecies;
        int newMaxHP = GetStat(StatType.HP);
        currentHP += newMaxHP - oldMaxHP;
        return true;
    }
}
