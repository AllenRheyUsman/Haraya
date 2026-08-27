using System.Collections.Generic;
using UnityEngine;

public class EncounterSpawner : MonoBehaviour
{
    [SerializeField] private List<PetSpecies> possibleSpecies = new();
    [SerializeField, Range(0, 1)] private float encounterChance = 0.1f;
    [SerializeField] private int minLevel = 2;
    [SerializeField] private int maxLevel = 5;

    public bool TryGenerateEncounter(out Pet encounter)
    {
        encounter = null;
        if (possibleSpecies.Count == 0 || Random.value > encounterChance)
        {
            return false;
        }

        var species = possibleSpecies[Random.Range(0, possibleSpecies.Count)];
        // TODO: generate real IVs/nature, roll starting moves from the movepool.
        encounter = new Pet
        {
            species = species,
            level = Random.Range(minLevel, maxLevel + 1)
        };

        return true;
    }
}
