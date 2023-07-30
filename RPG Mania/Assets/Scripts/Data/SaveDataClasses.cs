using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int level = 1;
    public List<int> skillActionsUses = new List<int>{0,0,0,0,0};
}

[System.Serializable]
public class WorldState
{
    public int currentScene = 1;
    public List<bool> enemyIsAliveWilds = new List<bool>();
    public List<bool> enemyIsAliveOutskirts = new List<bool>();
}

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public WorldState worldState;
}
