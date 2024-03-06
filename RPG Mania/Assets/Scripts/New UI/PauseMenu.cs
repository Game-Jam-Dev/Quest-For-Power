using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Progress;

public class PauseMenu : MonoBehaviour
{
    //[SerializeField]
    //TextMeshProUGUI itemNameTemplate;
    //[SerializeField]
    //TextMeshProUGUI itemQuantityTemplateText;


    Transform ItemContainer;
    Transform itemSlotTemplate;
    Transform itemQuantityTemplate;

    private void Awake()
    {
        ItemContainer = transform.Find("ItemsContainer");
        itemSlotTemplate = ItemContainer.Find("ItemSlotText");
        itemQuantityTemplate = ItemContainer.Find("ItemSlotQuantityTemplate");
    }

    public void DisplayItems()
    {
        
        Item[] items = ItemManager.GetInstance().GetItems();
        int y = 0;
        float itemSlotCellSize = 30f;

        foreach (Item item in items)
        {
            int itemAmount = GameManager.instance.GetItemAmount(item);

            if (itemAmount > 0)
            {
                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, ItemContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.anchoredPosition = new Vector2(38, -80 - y * itemSlotCellSize);
                RectTransform itemQtyRectTransform = Instantiate(itemQuantityTemplate, ItemContainer).GetComponent<RectTransform>();
                itemQtyRectTransform.gameObject.SetActive(true);
                itemQtyRectTransform.anchoredPosition = new Vector2(70, -8 - y * itemSlotCellSize);
                y++;
                itemSlotRectTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName;
                itemQtyRectTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = itemAmount.ToString();
            }
        }
    }

    public void ClearItems()
    {
        foreach (Transform child in ItemContainer)
        {
            if (child == itemSlotTemplate | child == itemQuantityTemplate) 
            {
                continue;
            }
            Destroy(child.gameObject);
        }
    }
}
