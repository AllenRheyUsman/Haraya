using System.Collections.Generic;
using UnityEngine;

public class CareManager : MonoBehaviour
{
    public static CareManager Instance { get; private set; }

    [SerializeField] private float hungerPerMinute = 1f;
    [SerializeField] private float hygieneDecayPerMinute = 0.5f;
    [SerializeField] private float maxStat = 100f;

    private readonly List<Pet> trackedPets = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        float minutesElapsed = Time.deltaTime / 60f;
        foreach (var pet in trackedPets)
        {
            Tick(pet, minutesElapsed);
        }
    }

    private void Tick(Pet pet, float minutesElapsed)
    {
        pet.hunger = Mathf.Min(maxStat, pet.hunger + hungerPerMinute * minutesElapsed);
        pet.hygiene = Mathf.Max(0f, pet.hygiene - hygieneDecayPerMinute * minutesElapsed);

        if (pet.hunger >= maxStat)
        {
            // TODO: apply starvation damage over time.
        }

        // TODO: sickness chance rises with low hygiene + high hunger; roll against StatusCondition.
    }

    public void RegisterPet(Pet pet)
    {
        if (!trackedPets.Contains(pet))
        {
            trackedPets.Add(pet);
        }
    }

    public void UnregisterPet(Pet pet)
    {
        trackedPets.Remove(pet);
    }

    public void Feed(Pet pet) => pet.hunger = 0f;

    public void Bathe(Pet pet) => pet.hygiene = maxStat;

    public void Rest(Pet pet) => pet.energy = maxStat;

    public void Play(Pet pet)
    {
        pet.happiness = Mathf.Min(255, pet.happiness + 5);
        // TODO: mini-game hook instead of an instant happiness bump.
    }
}
