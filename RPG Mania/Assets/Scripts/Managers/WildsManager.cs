using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class WildsManager : WorldManager {
    
    private List<EnemyInfo> battleEnemies = new List<EnemyInfo>();
    private List<EnemyInfo> liveEnemies = new List<EnemyInfo>();
    private List<EnemyInfo> reinforcements = new List<EnemyInfo>();
  
    [SerializeField] private GameObject mainCamera, battleCamera;
    private BattleCamera battleCameraScript;
  
    private AudioSource audioSource;
    [SerializeField] private AudioClip wildsTheme, battleTheme;
  
    [SerializeField] private DialogObject dialogObjectPre, dialogObjectPreboss, dialogObjectPost;
    private bool final = false;

    protected override void Start() {
        base.Start();

        battleCameraScript = battleCamera.GetComponent<BattleCamera>();

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
        foreach (EnemyInfo e in enemies)
        {
            e.InitializeEnemy(i);
            i++;

            if (e.GetIsAlive()) liveEnemies.Add(e);

            else Destroy(e.gameObject);
        }
    }

    public void BossFight(GameObject boss)
    {
        XixInfo xix = boss.GetComponent<XixInfo>();

        foreach (EnemyInfo e in reinforcements)
            xix.AddReinforcement(e);
        
        liveEnemies.Add(xix);

        final = true;

        StartCoroutine(BossFightDialog(boss));
        
    }

    public void EncounterEnemy(GameObject enemy, float rotationZ = 0)
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

        SwitchToBattleCamera();
        battleCameraScript.SetDirection(rotationZ);

        StartBattle();
    }

    private void StartBattle() 
    {
        playerInfo.PrepareCombat();

        foreach(EnemyInfo e in battleEnemies)
        {
            liveEnemies.Remove(e);
        }
        
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
            foreach (EnemyInfo e in liveEnemies)
            {
                e.gameObject.SetActive(true);
            }
        }

        playerInfo.GetPlayerAnimation().EscapeFromEnemy();

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

        playerInfo.ResetHealth();

        foreach (EnemyInfo e in battleEnemies)
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

    public override List<EnemyInfo> GetEnemies()
    {
        return battleEnemies;
    }
}