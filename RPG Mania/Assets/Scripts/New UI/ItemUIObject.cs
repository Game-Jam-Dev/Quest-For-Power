using UnityEngine;
using System.Text.RegularExpressions;

public class ItemUIObject : MonoBehaviour
{
    public string itemDescription;
    string itemName;

    GameObject itemDescriptionBox;

    private void Start()
    {
        itemDescriptionBox = GameObject.Find("ItemDescriptionText");
    }

    public void ClickOnText()
    {
        itemName = Regex.Split(GetComponent<TMPro.TextMeshProUGUI>().text, "\r\n")[0]; 
        Item[] items = ItemManager.GetInstance().GetItems();
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                itemDescription = item.description; 
                itemDescriptionBox.GetComponent<TMPro.TextMeshProUGUI>().text = itemDescription;
                break;
            }
        }        
    }
}
