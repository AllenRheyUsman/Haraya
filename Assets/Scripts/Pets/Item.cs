using System;

public enum ItemCategory
{
    Consumable,
    Battle,
    Held,
    Evolution,
    Key,
    Material
}

[Serializable]
public class Item
{
    public string itemId;
    public string itemName;
    public string description;
    public ItemCategory category;
    public int maxStack = 99;
}
