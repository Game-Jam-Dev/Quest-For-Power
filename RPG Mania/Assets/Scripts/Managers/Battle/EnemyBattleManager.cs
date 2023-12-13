using UnityEngine;

public class EnemyBattleManager : MonoBehaviour {
    [SerializeField] private ElementManager element;

    private EnemyBattle enemy;
    private EnemyContainerManager container;
    
    public void Initialize(EnemyContainerManager container, EnemyBattle enemy)
    {
        this.container = container;
        this.enemy = enemy;

        element.SetElement(enemy.element);
    }

    public void UpdateDamage()
    {

        if (enemy.health <= 0)
        {
            container.DefeatedEnemy(this);
        }
    }
}