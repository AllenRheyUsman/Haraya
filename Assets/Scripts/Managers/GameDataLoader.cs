using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpeciesDataFile
{
    public List<PetSpecies> species = new();
}

[Serializable]
public class ItemDataFile
{
    public List<Item> items = new();
}

public static class GameDataLoader
{
    private static readonly Dictionary<string, PetSpecies> speciesById = new();

    public static List<PetSpecies> LoadSpecies(TextAsset json)
    {
        var species = JsonUtility.FromJson<SpeciesDataFile>(json.text).species;
        foreach (var s in species)
        {
            speciesById[s.speciesId] = s;
        }
        return species;
    }

    public static List<Item> LoadItems(TextAsset json)
    {
        return JsonUtility.FromJson<ItemDataFile>(json.text).items;
    }

    // Populated by LoadSpecies - lets Pet resolve an evolvesIntoId without holding
    // a reference to whichever TextAsset the species data was originally loaded from.
    public static PetSpecies GetSpecies(string speciesId)
    {
        return speciesById.GetValueOrDefault(speciesId);
    }
}
