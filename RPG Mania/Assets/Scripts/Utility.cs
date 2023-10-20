using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Utility {
    
    public static void SetActiveButton(Button button)
    {
        if (button != null)
            EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}