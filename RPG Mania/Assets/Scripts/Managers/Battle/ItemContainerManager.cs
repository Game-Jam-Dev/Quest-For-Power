using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainerManager : MonoBehaviour {
    [SerializeField] private GameObject itemObjectPrefab;
    [SerializeField] private BattleUIManager uiManager;

    private List<(GameObject, Item)> itemObjects = new();

    public void Initialize()
    {
        Item[] items = ItemManager.GetInstance().GetItems();

        foreach (Item item in items)
        {
            GameObject itemObj = Instantiate(itemObjectPrefab, transform);

            itemObj.GetComponent<Button>().onClick.AddListener(() => UseItem(item));
            itemObj.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
            itemObjects.Add((itemObj, item));
            itemObj.SetActive(false);
        }

        GameObject itemObject = Instantiate(itemObjectPrefab, transform);

        itemObject.GetComponent<Button>().onClick.AddListener(Back);
        itemObject.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
    }
    public void UpdateItems()
    {
        if (itemObjects.Count == 0) Initialize();

        for (int i = 0; i < itemObjects.Count; i++)
        {
            GameObject skillObject = itemObjects[i].Item1;
            Item item = itemObjects[i].Item2;

            if (GameManager.instance.GetItems().Contains(item))
            {
                skillObject.SetActive(true);
            }
            else
            {
                skillObject.SetActive(false);
            }
        }
    }

    private void UseItem(Item item)
    {
        uiManager.PickItem(item);
    }

    private void Back()
    {
        uiManager.BackFromItem();
    }
}