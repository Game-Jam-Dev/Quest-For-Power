using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ThroneManager : MonoBehaviour {
    private GameObject gameController, player;
    private PlayerInfo playerInfo;
    private List<EnemyInfo> enemies = new List<EnemyInfo>();
    private GameManager gameManager;
    [SerializeField] private GameObject battleUI;
    private BattleThroneManager battleManager;
    private AudioSource audioSource;
    [SerializeField] private AudioClip throneTheme;
    private int wildsScene = 2;
    private bool bossFight = false;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();

        gameManager.SetPlayerExperience(490);

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerInfo = player.GetComponent<PlayerInfo>();

        battleManager = battleUI.GetComponentInChildren<BattleThroneManager>();

        SpawnEnemies();

        StartCoroutine(WaitAFrame());
    }

    

    private void SpawnEnemies()
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(e.GetComponent<EnemyInfo>());
        }
    }

    private void BossFight()
    {
        playerInfo.PrepareCombat();

        battleManager.enemies = new List<EnemyInfo>{GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyInfo>()};

        playerInfo.ResetHealth();

        battleUI.SetActive(true);
    }

    private void StartBattle() 
    {
        playerInfo.PrepareCombat();
        
        battleManager.enemies = enemies;
        playerInfo.ResetHealth();
        battleUI.SetActive(true);
    }

    public void EndSoldierBattle()
    {
        bossFight = true;
        StartCoroutine(WaitAFrame());
    }

    public void EndBossFight()
    {
        GameManager.instance.SetPlayerExperience(0);
        GameManager.instance.SetPlayerSkills(new List<int>{0,0,0,0,0});

        SceneManager.LoadScene(wildsScene);
    }

    public void EndBattle()
    {
        if (!bossFight) EndSoldierBattle();

        else EndBossFight();
    }

    private IEnumerator WaitAFrame() {
        yield return new WaitForEndOfFrame();

        if (!bossFight) StartBattle();
        
        else BossFight();
    }
}