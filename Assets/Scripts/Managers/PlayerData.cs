using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public string playerId;
    public string playerName;
    public int playerLevel = 1;
    public int currency;

    public List<Pet> party = new();
    public Dictionary<string, int> inventory = new();
}
