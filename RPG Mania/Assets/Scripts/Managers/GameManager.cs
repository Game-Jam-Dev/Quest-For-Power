using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public PlayerBattle player;
    private GameData gameData;
    private PlayerData playerData;
    private WorldState worldState;

    private readonly int wildsScene = 4;
    private readonly int outskirtsScene = 5;

    public void SetGameData(GameData gameData)
    {
        this.gameData = gameData;
        playerData = gameData.playerData;
        worldState = gameData.worldState;
    }

    public void SetPlayerExperience(int experience)
    {
        playerData.experience = experience;
    }

    public void SetPlayerLevel(int level)
    {
        playerData.level = level;
    }

    public int GetPlayerLevel()
    {
        return playerData.level;
    }

    public void SetPlayerSkills(List<int> skillActionUses)
    {
        playerData.skillActionUses = skillActionUses;
    }

    public void SetPlayerSkill(int i, int count)
    {
        playerData.skillActionUses[i] = count;
    }

    public List<int> GetPlayerSkillUses(){
        return playerData.skillActionUses;
    }
        

    public void SetCurrentScene(int currentScene)
    {
        worldState.currentScene = currentScene;
    }

    public void SetPlayerPosition(Vector2 playerPosition)
    {
        worldState.playerPosition = playerPosition;
    }

    public Vector2 GetPlayerPosition()
    {
        return worldState.playerPosition;
    }

    public bool CheckVisitedWilds()
    {
        return worldState.visitedWilds;
    }

    public void SetVisitedWilds(bool b)
    {
        worldState.visitedWilds = b;
    }

    public bool CheckEnemyDeath(int scene, int id)
    {
        if (scene == wildsScene) return worldState.enemyIsAliveWilds[id];

        else if (scene == outskirtsScene) return worldState.enemyIsAliveOutskirts[id];

        else return true;
    }

    public void SetEnemyDeath(int scene, int id)
    {
        if (scene == wildsScene) worldState.enemyIsAliveWilds[id] = false;

        else if (scene == outskirtsScene) worldState.enemyIsAliveOutskirts[id] = false;
    }

    public void SetWorldStateWilds(List<bool> enemyIsAliveWilds)
    {
        
        worldState.enemyIsAliveWilds = enemyIsAliveWilds;
    }

    public void SetWorldStateOutskirts(List<bool> enemyIsAliveOutskirts)
    {
        worldState.enemyIsAliveOutskirts = enemyIsAliveOutskirts;
    }

    public void AddItem(Item item)
    {
        playerData.itemQuantities[playerData.itemNames.IndexOf(item)]++;
    }

    public void AddItems(List<Item> items)
    {
        foreach (Item item in items)
        {
            AddItem(item);
        }
    }

    public void RemoveItem(Item item)
    {
        if (playerData.itemNames.Contains(item) && GetItemAmount(item) > 0) playerData.itemQuantities[playerData.itemNames.IndexOf(item)]--;
    }

    public void ClearItems()
    {
        for (int i = 0; i < playerData.itemQuantities.Count; i++)
        {
            playerData.itemQuantities[i] = 0;
        }
    }

    public int GetItemAmount(Item item)
    {
        if (playerData.itemNames.Contains(item)) return playerData.itemQuantities[playerData.itemNames.IndexOf(item)];

        else return 0;
    }

    public List<Item> GetItems()
    {
        return new List<Item>(playerData.itemNames);
    }

    public GameData GetGameData()
    {
        gameData = new GameData();
        gameData.playerData = playerData;
        gameData.worldState = worldState;

        return gameData;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player.GetComponent<PlayerBattle>();
        player.transform.position = worldState.playerPosition;
        this.player.SetData(playerData.level, playerData.experience, playerData.skillActionUses);
        this.player.ResetHealth();
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetGameData(new GameData().NewGame());
        }
        else
        {
            Destroy(gameObject);
        }
    }
}