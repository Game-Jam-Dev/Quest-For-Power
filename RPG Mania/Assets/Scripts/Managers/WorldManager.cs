using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour {
    [SerializeField] private Button battleButton;
    private GameObject gameController, player;
    private PlayerInfo playerInfo;
    private List<EnemyInfo> enemies = new List<EnemyInfo>();
    private List<EnemyInfo> battleEnemies = new List<EnemyInfo>();
    private GameManager gameManager;
    [SerializeField] private GameObject battleUI;
    private BattleWildsManager battleManager;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerInfo = player.GetComponent<PlayerInfo>();

        battleManager = battleUI.GetComponent<BattleWildsManager>();

        SpawnEnemies();

        battleButton.onClick.AddListener(StartBattle);
    }

    private void Update() {
        if (!battleUI.activeSelf) 
        {
            battleButton.gameObject.SetActive(true);
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

    private void StartBattle() {
        playerInfo.PrepareCombat();
        
        battleManager.enemies = battleEnemies;
        battleUI.SetActive(true);
        battleButton.gameObject.SetActive(false);
    }
}