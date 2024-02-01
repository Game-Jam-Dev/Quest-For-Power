using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleManager : MonoBehaviour {
    [SerializeField] private ElementManager element;
    private GameObject cursorObject;
    private Button cursorButton;

    private EnemyBattle enemy;
    private EnemyContainerManager container;
    
    
    public void Initialize(EnemyContainerManager container, EnemyBattle enemy, UIManager uiManager)
    {
        this.container = container;
        this.enemy = enemy;

        cursorObject = enemy.GetCursorDisplay();
        cursorObject.TryGetComponent(out cursorButton);

        // Canvas canvas = container.GetComponentInParent<Canvas>();

        // Vector3 enemyScreenPosition = Camera.main.WorldToScreenPoint(enemy.transform.position);
        // RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, enemyScreenPosition, canvas.worldCamera, out Vector3 enemyCanvasPosition);

        // cursorButton.transform.position = enemyCanvasPosition;

        cursorButton.onClick.AddListener(() => uiManager.PickTarget(this.enemy));

        element.SetElement(enemy.element);
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
            container.DefeatedEnemy(this);

        }


    }

    public EnemyBattle GetEnemy()
    {
        return enemy;
    }
}