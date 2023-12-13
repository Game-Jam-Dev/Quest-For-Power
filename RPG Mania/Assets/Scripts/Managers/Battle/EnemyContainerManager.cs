using System.Collections.Generic;
using UnityEngine;

public class EnemyContainerManager : MonoBehaviour {
    [SerializeField] private GameObject enemyDataPrefab; 

    private List<(EnemyBattle, EnemyBattleManager)> enemyDatas = new();
    
    public void SetEnemies(List<EnemyBattle> enemies)
    {
        ResetEnemies();

        foreach (EnemyBattle e in enemies)
        {
            CreateEnemy(e);
        }
    }

    private void CreateEnemy(EnemyBattle enemy)
    {
        EnemyBattleManager enemyData = Instantiate(enemyDataPrefab, transform).GetComponent<EnemyBattleManager>();
        enemyData.Initialize(this, enemy);

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
        e.gameObject.SetActive(false);
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
}