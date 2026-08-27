using UnityEngine;

public class TestBootstrap : MonoBehaviour
{
    [SerializeField] private TextAsset speciesData;

    private void Start()
    {
        if (speciesData == null)
        {
            Debug.LogError("TestBootstrap: speciesData TextAsset not assigned.");
            return;
        }

        var allSpecies = GameDataLoader.LoadSpecies(speciesData);
        var catSpecies = allSpecies.Find(s => s.speciesId == "cat");
        if (catSpecies == null)
        {
            Debug.LogError("TestBootstrap: 'cat' species not found in SpeciesData.json.");
            return;
        }

        var pet = new Pet
        {
            instanceId = System.Guid.NewGuid().ToString(),
            species = catSpecies,
            nickname = "Test Cat",
            level = 5,
            nature = Nature.Hardy
        };

        foreach (StatType stat in System.Enum.GetValues(typeof(StatType)))
        {
            pet.ivs[stat] = 31;
            pet.evs[stat] = 0;
        }
        pet.currentHP = pet.GetStat(StatType.HP);
        pet.InitializeStartingMoves();

        Debug.Log($"[TestBootstrap] Loaded {allSpecies.Count} species from JSON.");
        Debug.Log($"[TestBootstrap] Spawned '{pet.nickname}' ({catSpecies.speciesName}), Lv.{pet.level}");
        Debug.Log($"[TestBootstrap] HP:{pet.GetStat(StatType.HP)} Atk:{pet.GetStat(StatType.Attack)} " +
                  $"Def:{pet.GetStat(StatType.Defense)} SpA:{pet.GetStat(StatType.SpAttack)} " +
                  $"SpD:{pet.GetStat(StatType.SpDefense)} Spe:{pet.GetStat(StatType.Speed)}");
        Debug.Log($"[TestBootstrap] Known moves: {string.Join(", ", pet.knownMoves.ConvertAll(m => m.moveName))}");

        // Exercise the level curve, move-learning, and evolution in one grant so the whole
        // Pet & Stats pipeline gets verified before this phase is committed.
        pet.GainExperience(600);

        Debug.Log($"[TestBootstrap] After +600 XP: Lv.{pet.level}, species '{pet.species.speciesName}', " +
                  $"HP {pet.currentHP}/{pet.GetStat(StatType.HP)}");
        Debug.Log($"[TestBootstrap] Known moves: {string.Join(", ", pet.knownMoves.ConvertAll(m => m.moveName))}");

        if (CareManager.Instance != null)
        {
            CareManager.Instance.RegisterPet(pet);
            Debug.Log("[TestBootstrap] Registered pet with CareManager.");
        }
    }
}
