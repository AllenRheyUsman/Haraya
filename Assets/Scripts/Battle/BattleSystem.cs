using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState { Idle, PlayerTurn, EnemyTurn, Won, Lost, Fled }

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

        // TODO: play intro animation, show battle UI, trigger EnemyTurn if it goes first.
    }

    public void UseMove(Move move)
    {
        if (State != BattleState.PlayerTurn)
        {
            return;
        }

        ExecuteMove(playerPet, enemyPet, move);
        if (!CheckBattleEnd())
        {
            State = BattleState.EnemyTurn;
            // TODO: trigger enemy AI turn.
        }
    }

    private void ExecuteMove(Pet attacker, Pet defender, Move move)
    {
        int damage = DamageCalculator.CalculateDamage(attacker, defender, move);
        defender.currentHP = Mathf.Max(0, defender.currentHP - damage);
        // TODO: apply move.statusEffect via move.statusChance, play hit animation/VFX.
    }

    private bool CheckBattleEnd()
    {
        if (enemyPet.IsFainted)
        {
            State = BattleState.Won;
            return true;
        }

        if (playerPet.IsFainted)
        {
            State = BattleState.Lost;
            return true;
        }

        return false;
    }

    public void Flee()
    {
        State = BattleState.Fled;
        // TODO: flee chance based on speed, escape battle scene.
    }
}
