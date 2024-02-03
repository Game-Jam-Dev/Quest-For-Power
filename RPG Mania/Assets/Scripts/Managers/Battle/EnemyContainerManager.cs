using System.Collections.Generic;
using UnityEngine;

public class EnemyContainerManager : MonoBehaviour {
    [SerializeField] private GameObject enemyDataPrefab; 

    private List<(EnemyBattle, EnemyBattleManager)> enemyDatas = new();
    private EnemyBattleManager selectedEnemy;
    
    public void SetEnemies(List<EnemyBattle> enemies, BattleUIManager uiManager)
    {
        ResetEnemies();

        foreach (EnemyBattle e in enemies)
        {
            CreateEnemy(e, uiManager);
        }

        ResetSelectedEnemy();
    }

    private void CreateEnemy(EnemyBattle enemy, BattleUIManager uiManager)
    {
        EnemyBattleManager enemyData = Instantiate(enemyDataPrefab, transform).GetComponent<EnemyBattleManager>();
        enemyData.Initialize(this, enemy, uiManager);

        enemyDatas.Add((enemy, enemyData));
    }

    private void ResetEnemies()
    {
        enemyDatas.Clear();

        for (int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void ResetSelectedEnemy()
    {
        if (enemyDatas.Count > 0)
        {
            selectedEnemy = enemyDatas[0].Item2;
        }
        else
        {
            selectedEnemy = null;
        }
    }

    public void UpdateDamage(EnemyBattle enemy)
    {
        EnemyBattleManager enemyManager = FindEnemy(enemy).Item2;
        enemyManager.UpdateDamage();
        selectedEnemy = enemyManager;
    }

    public void DefeatedEnemy(EnemyBattleManager e)
    {
        enemyDatas.Remove(enemyDatas.Find(x => x.Item2 == e));
        ResetSelectedEnemy();
    }

    private (EnemyBattle, EnemyBattleManager) FindEnemy(EnemyBattle enemy)
    {
        foreach ((EnemyBattle, EnemyBattleManager) e in enemyDatas)
        {
            if (e.Item1 == enemy)
            {
                return e;
            }
        }

        return enemyDatas[0];
    }

    public void TargetEnemies()
    {
        foreach ((EnemyBattle, EnemyBattleManager) e in enemyDatas)
        {
            e.Item2.SelectEnemy();
        }
    }

    public void UntargetEnemies() {
        foreach ((EnemyBattle, EnemyBattleManager) e in enemyDatas)
        {
            e.Item2.DeselectEnemy();
        }
    }
}