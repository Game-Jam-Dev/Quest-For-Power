using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour {
    [SerializeField] private Button battleButton, bossButton, allButton;
    private GameObject gameController, player;
    private PlayerInfo playerInfo;
    private List<EnemyInfo> enemies = new List<EnemyInfo>();
    private List<EnemyInfo> battleEnemies = new List<EnemyInfo>();
    private GameManager gameManager;
    [SerializeField] private GameObject battleUI;
    private BattleWildsManager battleManager;
    private BattleThroneManager throneBattleManager;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerInfo = player.GetComponent<PlayerInfo>();

        battleManager = battleUI.GetComponent<BattleWildsManager>();
        throneBattleManager = battleUI.GetComponentInChildren<BattleThroneManager>();

        SpawnEnemies();

        if (battleButton != null)
        {
            battleButton.onClick.AddListener(StartBattle);
            bossButton.onClick.AddListener(BossFight);
            allButton.onClick.AddListener(FIghtAll);
        }   
    }

    private void Update() {
        if (!battleUI.activeSelf) 
        {
            if (battleButton != null)
            {
                battleButton.gameObject.SetActive(true);
                bossButton.gameObject.SetActive(true);
                allButton.gameObject.SetActive(true);
            }
            
            foreach (EnemyInfo e in enemies)
            {

                e.gameObject.SetActive(true);
            }
            
        }
    }

    private void SpawnEnemies()
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(e.GetComponent<EnemyInfo>());
        }
    }

    public void EncounterEnemy(GameObject enemy)
    {
        foreach (EnemyInfo e in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, e.gameObject.transform.position) <= e.detectRange)
            {
                battleEnemies.Add(e);
                e.PrepareCombat();
            } else 
            {
                e.gameObject.SetActive(false);
            }
        }

        foreach(EnemyInfo e in battleEnemies)
        {
            enemies.Remove(e);
        }

        StartBattle();
    }

    private void BossFight()
    {
        playerInfo.PrepareCombat();

        throneBattleManager.enemies = new List<EnemyInfo>{GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyInfo>()};
        playerInfo.SetStats(50);
        playerInfo.ResetHealth();

        battleUI.SetActive(true);
        battleButton.gameObject.SetActive(false);
        bossButton.gameObject.SetActive(false);
        allButton.gameObject.SetActive(false);    }

    private void FIghtAll()
    {
        enemies.Add(GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyInfo>());
        StartBattle();
    }

    private void StartBattle() {
        playerInfo.PrepareCombat();
        if (battleManager != null)
            battleManager.enemies = battleEnemies;
        else 
        {
            throneBattleManager.enemies = enemies;
            playerInfo.SetStats(50);
            playerInfo.ResetHealth();
            battleButton.gameObject.SetActive(false);
            bossButton.gameObject.SetActive(false);
            allButton.gameObject.SetActive(false);
        }
        
        battleUI.SetActive(true);
        
    }
}