using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WildsManager : MonoBehaviour {
    private GameObject gameController, player;
    private PlayerInfo playerInfo;
    private List<EnemyInfo> enemies = new List<EnemyInfo>();
    private List<EnemyInfo> battleEnemies = new List<EnemyInfo>();
    private List<EnemyInfo> liveEnemies = new List<EnemyInfo>();
    private List<EnemyInfo> reinforcements = new List<EnemyInfo>();
    private List<bool> livingEnemies;
    private GameManager gameManager;
    [SerializeField] private GameObject battleUI;
    private BattleWildsManager battleManager;
    private AudioSource audioSource;
    [SerializeField] private AudioClip wildsTheme, battleTheme;
    private int currentScene;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();

        audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerInfo = player.GetComponent<PlayerInfo>();

        battleManager = battleUI.GetComponent<BattleWildsManager>();



        currentScene = SceneManager.GetActiveScene().buildIndex;
        gameManager.SetCurrentScene(currentScene);

        SpawnEnemies();


    }

    private void SpawnEnemies()
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(e.GetComponent<EnemyInfo>());
        }

        int i = 0;
        foreach (EnemyInfo e in enemies)
        {
            e.InitializeEnemy(i);
            i++;

            if (e.isAlive) liveEnemies.Add(e);

            else Destroy(e.gameObject);
        }
    }

    public void BossFight(GameObject boss)
    {
        foreach (EnemyInfo e in reinforcements)
            boss.GetComponent<XixInfo>().AddReinforcement(e);

        EncounterEnemy(boss);
    }

    public void EncounterEnemy(GameObject enemy)
    {
        foreach (EnemyInfo e in liveEnemies)
        {
            if (Vector3.Distance(enemy.transform.position, e.gameObject.transform.position) <= e.detectRange)
            {
                battleEnemies.Add(e);
                if (!e.gameObject.activeSelf) e.gameObject.SetActive(true);
                e.PrepareCombat();
            } else 
            {
                e.gameObject.SetActive(false);
            }
        }

        foreach(EnemyInfo e in battleEnemies)
        {
            liveEnemies.Remove(e);
        }

        StartBattle();
    }

    private void StartBattle() {
        playerInfo.PrepareCombat();
        
        audioSource.clip = battleTheme;
        audioSource.time = 2.7f;
        audioSource.Play();
        battleUI.SetActive(true);
    }

    public void WinBattle()
    {
        if (enemies.Count > 0)
        {
            foreach (EnemyInfo e in liveEnemies)
            {
                e.gameObject.SetActive(true);
            }
        }

        battleEnemies.Clear();

        audioSource.clip = wildsTheme;
        audioSource.time = 0;
        audioSource.Play();
    }

    public void LoseBattle()
    {
        
    }

    public List<EnemyInfo> GetEnemies()
    {
        return battleEnemies;
    }
}