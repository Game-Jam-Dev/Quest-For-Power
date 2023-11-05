using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ThroneManager : WorldManager {
    [SerializeField] private Varian boss;
    private readonly string nextScene = "Wilds";
    private bool pre, preBoss = false;
    [SerializeField] private DialogObject dialogObjectStart, dialogObjectPreBoss, dialogObjectPostBoss;

    protected override void Start() {
        base.Start();

        gameManager.SetPlayerLevel(99);
        gameManager.SetPlayerSkills(new List<int>(Enumerable.Repeat(99, 5)));

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
        if (!pre) StartBattle();

        else if (!preBoss) BossFight();

        else NextScene();
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
        player.transform.position += Vector3.forward * 5;
        Camera.main.transform.position += Vector3.forward * 4;

        enemies = new List<EnemyBattle>{GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyBattle>()};

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