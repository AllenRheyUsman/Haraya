using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    private readonly Dictionary<string, int> itemCounts = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Add(Item item, int count = 1)
    {
        itemCounts.TryGetValue(item.itemId, out int current);
        itemCounts[item.itemId] = Mathf.Min(current + count, item.maxStack);
    }

    public bool Remove(Item item, int count = 1)
    {
        if (!itemCounts.TryGetValue(item.itemId, out int current) || current < count)
        {
            return false;
        }

        itemCounts[item.itemId] = current - count;
        return true;
    }

    public int GetCount(Item item)
    {
        return itemCounts.GetValueOrDefault(item.itemId, 0);
    }
}
