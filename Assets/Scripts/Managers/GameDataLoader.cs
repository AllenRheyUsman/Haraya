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
    public static List<PetSpecies> LoadSpecies(TextAsset json)
    {
        return JsonUtility.FromJson<SpeciesDataFile>(json.text).species;
    }

    public static List<Item> LoadItems(TextAsset json)
    {
        return JsonUtility.FromJson<ItemDataFile>(json.text).items;
    }
}
