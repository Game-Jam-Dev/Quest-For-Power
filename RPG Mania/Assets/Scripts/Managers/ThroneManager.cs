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
    private int wildsScene = 2;
    private bool bossFight = false;
    private bool exposition, pre, preBoss, post = true;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private List<DialogObject> dialogObjectsExpo, dialogObjectsPre, dialogObjectsPreBoss, dialogObjectsPost;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();

        gameManager.SetPlayerExperience(490);

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerInfo = player.GetComponent<PlayerInfo>();

        battleManager = battleUI.GetComponentInChildren<BattleThroneManager>();

        SpawnEnemies();

        StartCoroutine(DoDialogExposition(dialogObjectsExpo));
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
        player.transform.position += Vector3.forward * 5;
        Camera.main.transform.position += Vector3.forward * 4;

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
        StartCoroutine(DoDialogBoss(dialogObjectsPreBoss));
    }

    public void EndBossFight()
    {
        bossFight = false;
        StartCoroutine(DoDialogPost(dialogObjectsPost));
    }

    private void NextScene()
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

    private IEnumerator DoDialogExposition(List<DialogObject> dialogObjects)
    {
        dialogManager.enabled = true;

        foreach (DialogObject d in dialogObjects)
        {
            // Start the next dialog.
            dialogManager.DisplayDialog(d);

            // Wait for this dialog to finish before proceeding to the next one.
            yield return new WaitUntil(() => !dialogManager.ShowingDialog());
        }

        StartCoroutine(DoDialogPre(dialogObjectsPre));
    }

    private IEnumerator DoDialogPre(List<DialogObject> dialogObjects)
    {
        dialogManager.enabled = true;

        foreach (DialogObject d in dialogObjects)
        {
            // Start the next dialog.
            dialogManager.DisplayDialog(d);

            // Wait for this dialog to finish before proceeding to the next one.
            yield return new WaitUntil(() => !dialogManager.ShowingDialog());
        }

        StartBattle();

        dialogManager.enabled = false;
    }

    private IEnumerator DoDialogBoss(List<DialogObject> dialogObjects)
    {
        dialogManager.enabled = true;

        foreach (DialogObject d in dialogObjects)
        {
            // Start the next dialog.
            dialogManager.DisplayDialog(d);

            // Wait for this dialog to finish before proceeding to the next one.
            yield return new WaitUntil(() => !dialogManager.ShowingDialog());
        }

        BossFight();

        dialogManager.enabled = false;
    }

    private IEnumerator DoDialogPost(List<DialogObject> dialogObjects)
    {
        dialogManager.enabled = true;

        foreach (DialogObject d in dialogObjects)
        {
            // Start the next dialog.
            dialogManager.DisplayDialog(d);

            // Wait for this dialog to finish before proceeding to the next one.
            yield return new WaitUntil(() => !dialogManager.ShowingDialog());
        }

        dialogManager.enabled = false;

        NextScene();
    }
}