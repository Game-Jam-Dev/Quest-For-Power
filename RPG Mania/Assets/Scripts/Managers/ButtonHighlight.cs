using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite neutral, press;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        if(image == null)
        {
            Debug.LogError("No Image component found on this GameObject.");
        }
        else
        {
            image.sprite = neutral;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(image != null)
        {
            image.sprite = press;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(image != null)
        {
            image.sprite = neutral;
        }
    }
}
