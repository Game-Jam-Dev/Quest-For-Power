using System.Collections.Generic;
using UnityEngine;

public class EnemyContainerManager : MonoBehaviour {
    [SerializeField] private GameObject enemyDataPrefab; 

    private List<(EnemyBattle, EnemyBattleManager)> enemyDatas = new();
    
    public void SetEnemies(List<EnemyBattle> enemies, UIManager uiManager)
    {
        ResetEnemies();

        foreach (EnemyBattle e in enemies)
        {
            CreateEnemy(e, uiManager);
        }
    }

    private void CreateEnemy(EnemyBattle enemy, UIManager uiManager)
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

    public void UpdateDamage(EnemyBattle enemy)
    {
        FindEnemy(enemy).Item2.UpdateDamage();
    }

    public void DefeatedEnemy(EnemyBattleManager e)
    {
        enemyDatas.Remove(enemyDatas.Find(x => x.Item2 == e));
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

    public void TargetEnemies() {
        if (enemyDatas.Count > 0)
        {
            foreach ((EnemyBattle, EnemyBattleManager) e in enemyDatas)
            {
                e.Item2.LockCursor(false);
            }

            enemyDatas[0].Item2.SelectEnemy();
        }
    }

    public void UntargetEnemies() {
        foreach ((EnemyBattle, EnemyBattleManager) e in enemyDatas)
        {
            e.Item2.LockCursor(true);
        }
    }
}