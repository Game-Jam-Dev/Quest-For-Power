using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public int level = 1;
    public int experience = 0;
    public List<int> skillActionUses = new(Enumerable.Repeat(0,5));
    public IDictionary<Item, int> items = new Dictionary<Item, int>();
}

[System.Serializable]
public class WorldState
{
    public int currentScene = 1;
    public bool visitedWilds = false;
    public bool visitedOutskirts = false;
    public List<bool> enemyIsAliveWilds = new(Enumerable.Repeat(true, 14));
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

        return this;
    }
}