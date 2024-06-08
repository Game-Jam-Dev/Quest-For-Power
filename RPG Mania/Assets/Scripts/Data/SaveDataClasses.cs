using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level = 1;
    public int experience = 0;
    public List<int> skillActionUses = new(Enumerable.Repeat(0,5));
    public List<Item> itemNames = new();
    public List<int> itemQuantities = new();
}

[System.Serializable]
public class WorldState
{
    public int currentScene = 1;
    public Vector2 playerPosition = new();
    public bool visitedWilds = false;
    public bool visitedOutskirts = false;
    public List<bool> enemyIsAliveWilds = new(Enumerable.Repeat(true, 11));
    public List<bool> enemyIsAliveOutskirts = new(Enumerable.Repeat(true, 5));
}

[System.Serializable]
public class SettingsData
{
    public float volume = 1;
    public int qualityLevel = 1;
    public bool fullScreen = false;
}

[System.Serializable]
public class GameData
{
    public PlayerData playerData = new();
    public WorldState worldState = new();
    public SettingsData settingsData = new();

    public GameData NewGame()
    {
        playerData = new();
        worldState = new();

        Item[] itemList = Resources.LoadAll<Item>("Items");

        // sort items by child class Potion, Essence, StatChanger, then Bomb
        System.Array.Sort(itemList, (a, b) =>
        {
            if (a is Potion && !(b is Potion)) return -1;
            if (a is Essence && !(b is Potion || b is Essence)) return -1;
            if (a is StatChanger && !(b is Potion || b is Essence || b is StatChanger)) return -1;
            return 1;
        });

        foreach (Item item in itemList)
        {
            playerData.itemNames.Add(item);
            playerData.itemQuantities.Add(0);
        }

        return this;
    }
}