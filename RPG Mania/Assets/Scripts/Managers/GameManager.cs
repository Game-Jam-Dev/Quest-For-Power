using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public PlayerInfo player;
    private GameData gameData;
    private PlayerData playerData = new PlayerData();
    private WorldState worldState = new WorldState();

    public void SetGameData(GameData gameData)
    {
        playerData = gameData.playerData;
        worldState = gameData.worldState;
    }

    public void SetPlayerData(int level, List<int> skillActionsUses)
    {
        playerData.level = level;
        playerData.skillActionsUses = skillActionsUses;
    }

    public void SetCurrentScene(int currentScene)
    {
        worldState.currentScene = currentScene;
    }

    public bool CheckEnemyDeath(string scene, int id)
    {
        if (scene == "wilds") return worldState.enemyIsAliveWilds[id];

        else if (scene == "outskirts") return worldState.enemyIsAliveOutskirts[id];

        else return true;
    }

    public void SetEnemyDeath(string scene, int id)
    {
        if (scene == "wilds") worldState.enemyIsAliveWilds[id] = false;

        else if (scene == "outskirts") worldState.enemyIsAliveOutskirts[id] = false;

        Debug.Log(id + " " + worldState.enemyIsAliveWilds[id]);
    }

    public void SetWorldStateWilds(List<bool> enemyIsAliveWilds)
    {
        
        worldState.enemyIsAliveWilds = enemyIsAliveWilds;
    }

    public void SetWorldStateOutskirts(List<bool> enemyIsAliveOutskirts)
    {
        worldState.enemyIsAliveOutskirts = enemyIsAliveOutskirts;
    }

    public GameData GetGameData()
    {
        gameData = new GameData{playerData = this.playerData, worldState = this.worldState};
        return gameData;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player.GetComponent<PlayerInfo>();
        this.player.SetData(1);
        this.player.ResetHealth();
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}