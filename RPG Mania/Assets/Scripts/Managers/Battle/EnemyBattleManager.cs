using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleManager : MonoBehaviour {
    [SerializeField] private ElementManager element;
    [SerializeField] private Button cursorButton;
    public float cursorYOffset = 1f;

    private EnemyBattle enemy;
    private EnemyContainerManager container;
    
    
    public void Initialize(EnemyContainerManager container, EnemyBattle enemy, UIManager uiManager)
    {
        this.container = container;
        this.enemy = enemy;

        cursorButton.transform.position = enemy.transform.position;

        cursorButton.onClick.AddListener(() => uiManager.PickTarget(this.enemy));

        element.SetElement(enemy.element);
    }

    public void LockCursor(bool b)
    {
        cursorButton.interactable = !b;
    }

    public void SelectEnemy()
    {
        Utility.SetActiveButton(cursorButton);
    }

    public void UpdateDamage()
    {
        if (enemy.health <= 0)
        {
            cursorButton.gameObject.SetActive(false);
            
            container.DefeatedEnemy(this);
        }
    }

    public EnemyBattle GetEnemy()
    {
        return enemy;
    }
}