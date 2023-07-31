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

    public void SetPlayerExperience(int experience)
    {
        playerData.experience = experience;
    }

    public void SetPlayerSkills(List<int> skillActionUses)
    {
        playerData.skillActionUses = skillActionUses;
    }

    public void SetPlayerSkill(int i, int count)
    {
        playerData.skillActionUses[i] = count;
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

    public void TriggerOutskirtEvent()
    {
        worldState.outskirtEvent = false;
    }

    public GameData GetGameData()
    {
        gameData = new GameData{playerData = this.playerData, worldState = this.worldState};
        return gameData;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player.GetComponent<PlayerInfo>();
        this.player.SetData(playerData.experience, playerData.skillActionUses);
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