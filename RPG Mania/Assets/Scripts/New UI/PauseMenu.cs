using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Progress;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI itemListText;

    string currentItemListString;

    private void DisplayItems()
    {
        currentItemListString = "";

        Item[] items = ItemManager.GetInstance().GetItems();
        foreach (Item item in items)
        {
            int itemAmount = GameManager.instance.GetItemAmount(item);

            if (itemAmount > 0)
            {
                currentItemListString += item.itemName + "\n";
            }
        }

        itemListText.text = currentItemListString;
    }
}
