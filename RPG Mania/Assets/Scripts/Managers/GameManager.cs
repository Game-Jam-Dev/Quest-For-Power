using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public PlayerBattle player;
    private GameData gameData;
    private PlayerData playerData = new();
    private WorldState worldState = new();

    private readonly int wildsScene = 4;
    private readonly int outskirtsScene = 5;

    public void SetGameData(GameData gameData)
    {
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

    public void SetItemDictionary(IDictionary<Item, int> items)
    {
        playerData.items = items;
    }

    public void AddItem(Item item)
    {
        playerData.items[item]++;
    }

    public void AddItems(List<Item> items)
    {
        foreach (Item item in items)
        {
            playerData.items[item]++;
        }
    }

    public void RemoveItem(Item item)
    {
        if (playerData.items.ContainsKey(item) && playerData.items[item] > 0) playerData.items[item]--;
    }

    public int GetItemAmount(Item item)
    {
        if (playerData.items.ContainsKey(item)) return playerData.items[item];

        else return 0;
    }

    public List<Item> GetItems()
    {
        return (List<Item>)playerData.items.Keys;
    }

    public GameData GetGameData()
    {
        gameData = new GameData{playerData = playerData, worldState = worldState};
        return gameData;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player.GetComponent<PlayerBattle>();
        this.player.SetData(playerData.level, playerData.experience, playerData.skillActionUses);
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