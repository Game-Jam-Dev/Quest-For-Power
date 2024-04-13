using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class WildsManager : WorldManager {
    
    private List<EnemyBattle> battleEnemies = new List<EnemyBattle>();
    private List<EnemyBattle> liveEnemies = new List<EnemyBattle>();
    private List<EnemyBattle> reinforcements = new List<EnemyBattle>();
  
    [SerializeField] private GameObject mainCamera, battleCamera;
    private BattleCamera battleCameraScript;
  
    private AudioSource audioSource;
    [SerializeField] private AudioClip wildsTheme, battleTheme;
  
    [SerializeField] private DialogObject dialogObjectPre, dialogObjectPreboss, dialogObjectPost;
    private bool final = false;

    protected override void Start() {
        base.Start();

        if (battleCamera != null )
        {
            battleCameraScript = battleCamera.GetComponent<BattleCamera>();
        } 
        else
        {
            return;
        }

        audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();

        if (!GameManager.instance.CheckVisitedWilds())
        {
            GameManager.instance.SetVisitedWilds(true);
            StartCoroutine(DoDialog(dialogObjectPre));
        } else 
        {
            dialogManager.enabled = false;
        }
    }

    protected override void SpawnEnemies()
    {
        base.SpawnEnemies();

        int i = 0;
        foreach (EnemyBattle e in enemies)
        {
            e.InitializeEnemy(i);
            i++;

            if (e.GetIsAlive()) liveEnemies.Add(e);

            else Destroy(e.gameObject);
        }
    }

    public void BossFight(GameObject boss)
    {
        Xix xix = boss.GetComponent<Xix>();

        foreach (EnemyBattle e in reinforcements)
            xix.AddReinforcement(e);
        
        liveEnemies.Add(xix);

        final = true;
        canEscape = false;
        hasResolutionUI = false;

        StartCoroutine(BossFightDialog(boss));
        
    }

    public void EncounterEnemy(GameObject enemy, float rotationZ = 0)
    {
        foreach (EnemyBattle e in liveEnemies)
        {
            if (Vector3.Distance(enemy.transform.position, e.gameObject.transform.position) <= e.detectRange)
            {
                battleEnemies.Add(e);
                if (!e.gameObject.activeSelf) e.gameObject.SetActive(true);
            } else 
            {
                e.gameObject.SetActive(false);
            }
        }

        SwitchToBattleCamera();
        battleCameraScript.SetDirection(rotationZ);

        StartBattle();
    }

    private void StartBattle() 
    {
        playerBattle.PrepareCombat();

        foreach(EnemyBattle e in battleEnemies)
        {
            liveEnemies.Remove(e);
            e.PrepareCombat();
        }
        
        battleEnemies.Reverse();

        audioSource.clip = battleTheme;
        audioSource.time = 2.7f;
        audioSource.Play();
        battleUI.SetActive(true);
    }

    private void SwitchToBattleCamera()
    {
        battleCamera.SetActive(true);
        mainCamera.SetActive(false);
    }

    private void SwitchToMainCamera()
    {
        mainCamera.SetActive(true);
        battleCamera.SetActive(false);
    }

    private void EndBattle()
    {
        if (enemies.Count > 0)
        {
            foreach (EnemyBattle e in liveEnemies)
            {
                e.gameObject.SetActive(true);
            }
        }

        playerBattle.GetPlayerAnimation().EscapeFromEnemy();

        SwitchToMainCamera();

        audioSource.clip = wildsTheme;
        audioSource.time = 0;
        audioSource.Play();
    }

    public override void WinBattle()
    {
        battleEnemies.Clear();
        
        if (final)
        {
            StartCoroutine(BossFightWinDialog());
        }

        EndBattle();
    }

    public override void EscapeBattle()
    {
        EndBattle();

        playerBattle.ResetHealth();

        foreach (EnemyBattle e in battleEnemies)
        {
            e.ResetFromFight();
            liveEnemies.Add(e);
        }
    }

    private IEnumerator BossFightDialog(GameObject boss)
    {
        StartCoroutine(DoDialog(dialogObjectPreboss));

        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => !dialogManager.enabled);

        EncounterEnemy(boss);
    }

    private IEnumerator BossFightWinDialog()
    {
        StartCoroutine(DoDialog(dialogObjectPost));

        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => !dialogManager.enabled);

        SceneManager.LoadScene("Credits");
    }

    public override List<EnemyBattle> GetBattleEnemies()
    {
        return battleEnemies;
    }
}