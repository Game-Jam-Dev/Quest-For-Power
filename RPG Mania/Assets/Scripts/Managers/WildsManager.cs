using UnityEngine;
using System.Collections.Generic;

public class WildsManager : MonoBehaviour {
    private GameObject gameController, player;
    private PlayerInfo playerInfo;
    private List<EnemyInfo> enemies = new List<EnemyInfo>();
    private List<EnemyInfo> battleEnemies = new List<EnemyInfo>();
    private GameManager gameManager;
    [SerializeField] private GameObject battleUI;
    private BattleWildsManager battleManager;
    private AudioSource audioSource;
    [SerializeField] private AudioClip wildsTheme, battleTheme;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();

        audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerInfo = player.GetComponent<PlayerInfo>();

        battleManager = battleUI.GetComponent<BattleWildsManager>();

        SpawnEnemies();
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
        if (battleManager != null)
            battleManager.enemies = battleEnemies;
        
        audioSource.clip = battleTheme;
        audioSource.time = 2.7f;
        audioSource.Play();
        battleUI.SetActive(true);
    }

    public void EndBattle()
    {
        if (enemies.Count > 0)
        {
            foreach (EnemyInfo e in enemies)
            {
                e.gameObject.SetActive(true);
            }
        }

        audioSource.clip = wildsTheme;
        audioSource.time = 0;
        audioSource.Play();
    }
}