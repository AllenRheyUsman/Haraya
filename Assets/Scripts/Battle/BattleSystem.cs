using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState { Idle, PlayerTurn, EnemyTurn, Won, Lost, Fled }

    private const float ParalysisFailChance = 0.25f;
    private const float SleepWakeChance = 0.33f;
    private const float FreezeThawChance = 0.2f;
    private const int StatusDamageDivisor = 8;

    public BattleState State { get; private set; } = BattleState.Idle;

    private Pet playerPet;
    private Pet enemyPet;

    public void StartBattle(Pet player, Pet enemy)
    {
        playerPet = player;
        enemyPet = enemy;
        State = player.GetStat(StatType.Speed) >= enemy.GetStat(StatType.Speed)
            ? BattleState.PlayerTurn
            : BattleState.EnemyTurn;

        if (State == BattleState.EnemyTurn)
        {
            RunEnemyTurn();
        }
    }

    public void UseMove(Move move)
    {
        if (State != BattleState.PlayerTurn)
        {
            return;
        }

        if (TryActThisTurn(playerPet))
        {
            ExecuteMove(playerPet, enemyPet, move);
        }

        ApplyEndOfTurnStatusDamage(playerPet);

        if (CheckBattleEnd())
        {
            return;
        }

        State = BattleState.EnemyTurn;
        RunEnemyTurn();
    }

    private void RunEnemyTurn()
    {
        if (enemyPet.knownMoves.Count > 0 && TryActThisTurn(enemyPet))
        {
            var move = enemyPet.knownMoves[Random.Range(0, enemyPet.knownMoves.Count)];
            ExecuteMove(enemyPet, playerPet, move);
        }

        ApplyEndOfTurnStatusDamage(enemyPet);

        if (!CheckBattleEnd())
        {
            State = BattleState.PlayerTurn;
        }
    }

    // Returns false (and logs why) when a status condition prevents the pet from acting this turn.
    private bool TryActThisTurn(Pet pet)
    {
        switch (pet.status)
        {
            case StatusCondition.Paralysis:
                if (Random.value < ParalysisFailChance)
                {
                    Debug.Log($"{pet.nickname} is paralyzed and can't move!");
                    return false;
                }
                return true;

            case StatusCondition.Sleep:
                if (Random.value < SleepWakeChance)
                {
                    pet.status = StatusCondition.None;
                    Debug.Log($"{pet.nickname} woke up!");
                    return true;
                }
                Debug.Log($"{pet.nickname} is fast asleep.");
                return false;

            case StatusCondition.Freeze:
                if (Random.value < FreezeThawChance)
                {
                    pet.status = StatusCondition.None;
                    Debug.Log($"{pet.nickname} thawed out!");
                    return true;
                }
                Debug.Log($"{pet.nickname} is frozen solid!");
                return false;

            default:
                return true;
        }
    }

    private void ExecuteMove(Pet attacker, Pet defender, Move move)
    {
        float effectiveness = TypeChart.GetEffectiveness(move.type, defender.species.primaryType, defender.species.secondaryType);
        int damage = DamageCalculator.CalculateDamage(attacker, defender, move);
        defender.currentHP = Mathf.Max(0, defender.currentHP - damage);

        Debug.Log($"{attacker.nickname} used {move.moveName}! It dealt {damage} damage to {defender.nickname} " +
                  $"({defender.currentHP}/{defender.GetStat(StatType.HP)} HP left).");

        if (effectiveness > 1f) Debug.Log("It's super effective!");
        else if (effectiveness < 1f && effectiveness > 0f) Debug.Log("It's not very effective...");
        else if (effectiveness == 0f) Debug.Log("It had no effect...");

        if (move.statusEffect != StatusCondition.None && defender.status == StatusCondition.None && !defender.IsFainted)
        {
            if (Random.value < move.statusChance)
            {
                defender.status = move.statusEffect;
                Debug.Log($"{defender.nickname} was afflicted with {move.statusEffect}!");
            }
        }
    }

    private void ApplyEndOfTurnStatusDamage(Pet pet)
    {
        if (pet.IsFainted || (pet.status != StatusCondition.Poison && pet.status != StatusCondition.Burn))
        {
            return;
        }

        int damage = Mathf.Max(1, pet.GetStat(StatType.HP) / StatusDamageDivisor);
        pet.currentHP = Mathf.Max(0, pet.currentHP - damage);
        Debug.Log($"{pet.nickname} is hurt by its {pet.status}! ({damage} damage, {pet.currentHP}/{pet.GetStat(StatType.HP)} HP left).");
    }

    private bool CheckBattleEnd()
    {
        if (enemyPet.IsFainted)
        {
            State = BattleState.Won;
            Debug.Log($"{enemyPet.nickname} fainted! You won the battle!");
            return true;
        }

        if (playerPet.IsFainted)
        {
            State = BattleState.Lost;
            Debug.Log($"{playerPet.nickname} fainted! You lost the battle.");
            return true;
        }

        return false;
    }

    public void Flee()
    {
        State = BattleState.Fled;
        Debug.Log($"{playerPet.nickname} fled from the battle.");
        // TODO: flee chance based on speed instead of always succeeding.
    }
}
