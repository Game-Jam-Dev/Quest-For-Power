using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;

public class ItemUIObject : MonoBehaviour
{
    public string itemDescription;
    string itemName;

    GameObject itemDescriptionBox;

    private void Start()
    {
        itemDescriptionBox = GameObject.Find("ItemDescriptionText");
    }

    public void TestClick()
    {
        itemName = Regex.Split(GetComponent<TMPro.TextMeshProUGUI>().text, "\r\n")[0]; 
        Debug.Log("This item name: " + itemName);
        Debug.Log(itemName.Length);
        Item[] items = ItemManager.GetInstance().GetItems();
        Debug.Log(items);
        foreach (Item item in items)
        {
            Debug.Log("Item list object: " + item.itemName);
            Debug.Log(item.itemName.Length);
            Debug.Log(item.itemName == itemName);
            if (item.itemName == itemName)
            {
                Debug.Log("Found match");
                itemDescription = item.description; 
                Debug.Log(itemDescription);
                itemDescriptionBox.GetComponent<TMPro.TextMeshProUGUI>().text = itemDescription;
                break;
            }
        }        
    }
}
