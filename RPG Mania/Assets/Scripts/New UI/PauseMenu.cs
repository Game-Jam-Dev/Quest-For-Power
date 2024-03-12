using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Progress;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI itemDescription;


    Transform itemContainer;
    Transform itemSlotTemplate;
    Transform itemQuantityTemplate;

    Transform spellsContainer;
    Transform spellsSlotTemplate;
    Transform spellsQuantityTemplate;

    private void Awake()
    {
        itemContainer = transform.Find("ItemsContainer");
        itemSlotTemplate = itemContainer.Find("ItemSlotText");
        itemQuantityTemplate = itemContainer.Find("ItemSlotQuantityTemplate");

        spellsContainer = transform.Find("SpellList");
        spellsSlotTemplate = spellsContainer.Find("SpellSlotText");
        spellsQuantityTemplate = spellsContainer.Find("SpellSlotQuantityTemplate");
    }

    public void DisplaySpells()
    {

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
                if (y == 0)
                {
                    //itemDescription.textStyle = 
                }

                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.anchoredPosition = new Vector2(38, -159 - y * itemSlotCellSize);
                RectTransform itemQtyRectTransform = Instantiate(itemQuantityTemplate, itemContainer).GetComponent<RectTransform>();
                itemQtyRectTransform.gameObject.SetActive(true);
                itemQtyRectTransform.anchoredPosition = new Vector2(70, -87 - y * itemSlotCellSize);
                y++;
                itemSlotRectTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName;
                itemQtyRectTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = itemAmount.ToString();

                // click for Item description
            }
        }
    }

    public void ClearItems()
    {
        foreach (Transform child in itemContainer)
        {
            if (child == itemSlotTemplate | child == itemQuantityTemplate) 
            {
                continue;
            }
            Destroy(child.gameObject);
        }
    }
}
