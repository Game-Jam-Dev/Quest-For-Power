using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ThroneManager : WorldManager {
    private GameObject gameController, player;
    private PlayerInfo playerInfo;
    private List<EnemyInfo> enemies = new List<EnemyInfo>();
    [SerializeField] private VarianInfo boss;
    private GameManager gameManager;
    [SerializeField] private GameObject battleUI;
    private string wildsScene = "Wilds";
    private bool pre, preBoss = false;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private DialogObject dialogObjectStart, dialogObjectPreBoss, dialogObjectPostBoss;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();

        gameManager.SetPlayerLevel(99);
        gameManager.SetPlayerSkills(new List<int>(Enumerable.Repeat(99, 5)));

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerInfo = player.GetComponent<PlayerInfo>();

        SpawnEnemies();

        StartCoroutine(DoDialog(dialogObjectStart));
    }

    private IEnumerator DoDialog(DialogObject dialogObject)
    {
        dialogManager.enabled = true;
        
        // Start the next dialog.
        dialogManager.DisplayDialog(dialogObject);

        // Wait for this dialog to finish before proceeding to the next one.
        yield return new WaitUntil(() => !dialogManager.ShowingDialog());
        
        dialogManager.enabled = false;

        NextBattle();
    }

    private void NextBattle()
    {
        if (!pre) StartBattle();

        else if (!preBoss) BossFight();

        else NextScene();
    }

    private void StartBattle() 
    {
        playerInfo.PrepareCombat();
        
        foreach (EnemyInfo e in enemies) e.PrepareCombat();

        playerInfo.ResetHealth();
        battleUI.SetActive(true);
    }

    private void BossFight()
    {
        player.transform.position += Vector3.forward * 5;
        Camera.main.transform.position += Vector3.forward * 4;

        enemies = new List<EnemyInfo>{GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyInfo>()};

        StartBattle();
    }

    public override void WinBattle()
    {
        if (!pre) EndSoldierBattle();

        else EndBossFight();
    }

    private void EndSoldierBattle()
    {
        pre = true;
        StartCoroutine(DoDialog(dialogObjectPreBoss));
    }

    private void EndBossFight()
    {
        preBoss = true;
        StartCoroutine(DoDialog(dialogObjectPostBoss));
    }

    

    private void SpawnEnemies()
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(e.GetComponent<EnemyInfo>());
        }
    }

    private void NextScene()
    {
        GameManager.instance.SetPlayerLevel(1);
        GameManager.instance.SetPlayerExperience(0);
        GameManager.instance.SetPlayerSkills(new List<int>{0,0,0,0,0});

        SceneManager.LoadScene(wildsScene);
    }

    private IEnumerator SoldierFightNoDialog()
    {
        yield return new WaitForEndOfFrame();

        StartBattle();
    }

    private IEnumerator BossFightNoDialog()
    {
        yield return new WaitForEndOfFrame();

        BossFight();
    }

    public override List<EnemyInfo> GetEnemies()
    {
        return enemies;
    }
}