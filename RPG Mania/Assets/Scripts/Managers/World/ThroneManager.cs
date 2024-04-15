
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ThroneManager : WorldManager {
    private readonly string nextScene = "Throne Cutscene";
    private bool preBattle = false;
    [SerializeField] private DialogObject dialogObjectStart, dialogObjectPreBoss;
    [SerializeField] private GameObject varian;

    protected override void Start() {
        base.Start();

        gameManager.SetPlayerLevel(40);
        gameManager.SetPlayerSkills(new List<int>(Enumerable.Repeat(40, 5)));

        gameManager.SetPlayer(player);

        StartCoroutine(DoDialog(dialogObjectStart));
    }

    protected override IEnumerator DoDialog(DialogObject dialogObject)
    {
        StartCoroutine(base.DoDialog(dialogObject));

        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => !dialogManager.enabled);

        NextBattle();
    }

    private void NextBattle()
    {
        if (!preBattle) StartBattle();

        else BossFight();
    }

    private void StartBattle() 
    {
        playerBattle.PrepareCombat();
        
        foreach (EnemyBattle e in enemies) e.PrepareCombat();

        playerBattle.ResetHealth();
        battleUI.SetActive(true);
    }

    private void BossFight()
    {
        player.transform.position += Vector3.forward * 5f;
        //varian.transform.position += Vector3.forward * 1.5f;
        Camera.main.transform.position += Vector3.forward * 4f;
        //Camera.main.transform.Rotate(10, 0, 0);

        enemies = new List<EnemyBattle>{GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyBattle>()};

        StartBattle();
    }

    public override void WinBattle()
    {
        if (!preBattle) EndSoldierBattle();

        else NextScene();
    }

    private void EndSoldierBattle()
    {
        preBattle = true;
        StartCoroutine(DoDialog(dialogObjectPreBoss));
    }

    private void NextScene()
    {
        GameManager.instance.SetPlayerLevel(1);
        GameManager.instance.SetPlayerExperience(0);
        GameManager.instance.SetPlayerSkills(new List<int>{0,0,0,0,0});

        SceneManager.LoadScene(nextScene);
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
}