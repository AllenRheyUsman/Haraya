using System;
using UnityEngine;

[Serializable]
public class Move
{
    public string moveName;
    public ElementType type;
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
