using System;
using System.Text;
using UnityEngine;

public class BattleTestHarness : MonoBehaviour
{
    [SerializeField] private TextAsset speciesData;

    private BattleSystem battle;
    private Pet playerPet;
    private Pet enemyPet;

    private void Start()
    {
        if (speciesData == null)
        {
            Debug.LogError("BattleTestHarness: speciesData TextAsset not assigned.");
            return;
        }

        var allSpecies = GameDataLoader.LoadSpecies(speciesData);
        var catSpecies = allSpecies.Find(s => s.speciesId == "cat");
        if (catSpecies == null)
        {
            Debug.LogError("BattleTestHarness: 'cat' species not found.");
            return;
        }

        playerPet = CreatePet(catSpecies, "Player Cat", level: 8, nature: Nature.Adamant);
        enemyPet = CreatePet(catSpecies, "Wild Cat", level: 4, nature: Nature.Hardy);

        battle = gameObject.AddComponent<BattleSystem>();

        Debug.Log("=== BATTLE START ===");
        Debug.Log($"{playerPet.nickname} (Lv.{playerPet.level}, {playerPet.currentHP} HP) vs " +
                  $"{enemyPet.nickname} (Lv.{enemyPet.level}, {enemyPet.currentHP} HP)");

        battle.StartBattle(playerPet, enemyPet);
        AnnounceTurnIfPlayer();
    }

    private Pet CreatePet(PetSpecies species, string nickname, int level, Nature nature)
    {
        var pet = new Pet
        {
            instanceId = Guid.NewGuid().ToString(),
            species = species,
            nickname = nickname,
            level = level,
            nature = nature
        };

        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            pet.ivs[stat] = 31;
            pet.evs[stat] = 0;
        }

        pet.currentHP = pet.GetStat(StatType.HP);
        pet.InitializeStartingMoves();
        return pet;
    }

    private void Update()
    {
        if (battle == null || battle.State != BattleSystem.BattleState.PlayerTurn)
        {
            return;
        }

        for (int i = 0; i < playerPet.knownMoves.Count && i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                battle.UseMove(playerPet.knownMoves[i]);
                AnnounceTurnIfPlayer();
                break;
            }
        }
    }

    private void AnnounceTurnIfPlayer()
    {
        if (battle.State != BattleSystem.BattleState.PlayerTurn)
        {
            return;
        }

        var options = new StringBuilder("Choose a move: ");
        for (int i = 0; i < playerPet.knownMoves.Count; i++)
        {
            options.Append($"[{i + 1}] {playerPet.knownMoves[i].moveName}  ");
        }
        Debug.Log(options.ToString());
    }
}
