using UnityEngine;

public class ItemManager {
    public static ItemManager Instance { get; private set; }
    private readonly Item[] items;

    public ItemManager()
    {
        items = Resources.LoadAll<Item>("Items");
    }

    public static ItemManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new ItemManager();
        }

        return Instance;
    }

    public Item GetItem(int index)
    {
        if (index < items.Length) return items[index];

        else return null;
    }

    public Item[] GetItems() { return items; }
}