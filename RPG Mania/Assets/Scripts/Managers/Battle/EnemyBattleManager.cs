using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleManager : MonoBehaviour {
    [SerializeField] private ElementManager element;
    [SerializeField] private EnemyHealthManager healthManager;
    private GameObject cursorObject;
    private Button cursorButton;

    private EnemyBattle enemy;
    private EnemyContainerManager container;
    
    public void Initialize(EnemyContainerManager container, EnemyBattle enemy, UIManager uiManager)
    {
        this.container = container;
        this.enemy = enemy;

        healthManager.Initialize(enemy);
        element.SetElement(enemy.element);

        cursorObject = enemy.GetCursorDisplay();
        cursorObject.TryGetComponent(out cursorButton);
        cursorButton.onClick.AddListener(() => uiManager.PickTarget(this.enemy));
    }

    public void DeselectEnemy()
    {
        cursorObject.SetActive(false);
    }

    public void SelectEnemy()
    {
        cursorObject.SetActive(true);
    }

    public void SelectButton()
    {
        Utility.SetActiveButton(cursorButton);
    }

    public void UpdateDamage()
    {
        if (enemy.health <= 0)
        {
            healthManager.Defeated();
            container.DefeatedEnemy(this);
        } else
        {
            healthManager.UpdateHealth(enemy.health);
        }
    }

    public EnemyBattle GetEnemy()
    {
        return enemy;
    }
}