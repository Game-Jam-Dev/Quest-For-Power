using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CursorHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private GameObject cursor;
    public float cursorChange = .25f;
    private EnemyBattleManager enemyBattleManager;

    private bool isHovering = false;
    

    public void ConnectUI(EnemyBattleManager enemyBattleManager)
    {
        this.enemyBattleManager = enemyBattleManager;
    }

    public void ResetCursor()
    {
        if (isHovering)
        {
            OnPointerExit();
        }
    }

    // This function is called when the mouse enters the UI element.
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Enter");
        cursor.transform.position += new Vector3(0, cursorChange, 0);
        enemyBattleManager.SelectButton();
        isHovering = true;
    }

    // This function is called when the mouse exits the UI element.
    public void OnPointerExit(PointerEventData eventData = null)
    {
        Debug.Log("Mouse Exit");
        cursor.transform.position -= new Vector3(0, cursorChange, 0);
        enemyBattleManager.DeselectButton();
        isHovering = false;
    }
}