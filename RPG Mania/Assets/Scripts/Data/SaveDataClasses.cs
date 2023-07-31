using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public int experience = 490;
    public List<int> skillActionUses = new List<int>{0,0,0,0,0};
}

[System.Serializable]
public class WorldState
{
    public int currentScene = 1;
    public List<bool> enemyIsAliveWilds = new List<bool>(System.Linq.Enumerable.Repeat(true, 14));
    public List<bool> enemyIsAliveOutskirts = new List<bool>(System.Linq.Enumerable.Repeat(true, 5));
    public bool outskirtEvent = true;
}

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public WorldState worldState;
}
