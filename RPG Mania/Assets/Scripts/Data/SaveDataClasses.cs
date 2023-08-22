using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public int level = 1;
    public int experience = 0;
    public List<int> skillActionUses = new List<int>(Enumerable.Repeat(0,5));
}

[System.Serializable]
public class WorldState
{
    public int currentScene = 1;
    public List<bool> enemyIsAliveWilds = new List<bool>(Enumerable.Repeat(true, 14));
    public List<bool> enemyIsAliveOutskirts = new List<bool>(Enumerable.Repeat(true, 5));
}

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public WorldState worldState;
}
