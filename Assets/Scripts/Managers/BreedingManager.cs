using System.Collections.Generic;
using UnityEngine;

public class BreedingManager : MonoBehaviour
{
    public static BreedingManager Instance { get; private set; }

    [SerializeField] private float incubationHours = 3f;

    private readonly Dictionary<(Pet a, Pet b), float> activeBreeding = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool CanBreed(Pet a, Pet b)
    {
        if (a == null || b == null || a == b || a.IsFainted || b.IsFainted)
        {
            return false;
        }

        if (a.status != StatusCondition.None || b.status != StatusCondition.None)
        {
            return false;
        }

        // TODO: real compatibility check (egg groups / defined breeding pairs).
        return true;
    }

    public void StartBreeding(Pet a, Pet b)
    {
        if (!CanBreed(a, b))
        {
            return;
        }

        activeBreeding[(a, b)] = incubationHours * 3600f;
        // TODO: persist breeding timer across sessions (real-time, per the scope doc).
    }

    private Pet GenerateOffspring(Pet parentA, Pet parentB)
    {
        // TODO: average + variation on base stats, random/inherited nature,
        // one inherited ability, egg-move inheritance, shiny roll.
        return new Pet
        {
            species = parentA.species,
            level = 1
        };
    }
}
