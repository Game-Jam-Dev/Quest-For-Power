using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainerManager : MonoBehaviour {
    [SerializeField] private GameObject itemObjectPrefab;
    [SerializeField] private UIManager uiManager;

    private List<(GameObject, Item)> itemObjects = new();

    private void Start()
    {
        Item[] items = ItemManager.GetInstance().GetItems();

        foreach (Item item in items)
        {
            GameObject itemObject = Instantiate(itemObjectPrefab, transform);

            itemObject.GetComponent<Button>().onClick.AddListener(() => UseItem(item));
            itemObject.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
            itemObjects.Add((itemObject, item));
            itemObject.SetActive(false);
        }
    }
    public void UpdateItems()
    {
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
}