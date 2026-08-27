using System;
using UnityEngine;

public enum MoveCategory { Physical, Special }

[Serializable]
public class Move
{
    public string moveName;
    public ElementType type;
    public MoveCategory category = MoveCategory.Physical;
    public int power;
    public int accuracy;
    public int maxPP;
    public int currentPP;
    public StatusCondition statusEffect;
    [Range(0, 1)] public float statusChance;

    public Move Clone()
    {
        return (Move)MemberwiseClone();
    }
}
