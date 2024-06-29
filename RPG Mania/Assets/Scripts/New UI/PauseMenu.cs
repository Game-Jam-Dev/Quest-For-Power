using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using static UnityEditor.Progress;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI description;

    [SerializeField]
    TextMeshProUGUI lvlValue, xp, xpForNextLvl, CPValue, HPValue, AttValue, DefValue,
        AccValue, EvaValue;


    Transform itemGroup;
    Transform itemSlotTemplate;
    Transform itemQuantityTemplate;

    Transform spellsGroup;
    Transform spellsContainer;
    Transform spellsSlotTemplate;
    Transform spellsQuantityTemplate;

    GameObject player;

    private void Start()
    {
        itemGroup = transform.Find("Items").Find("Items container");
        itemSlotTemplate = itemGroup.Find("ItemSlotText");
        itemQuantityTemplate = itemGroup.Find("ItemSlotQuantityTemplate");

        spellsGroup = transform.Find("Spells").transform.Find("Spells container");
        spellsSlotTemplate = spellsGroup.Find("SpellSlotText");
        spellsQuantityTemplate = spellsGroup.Find("SpellSlotQuantityTemplate");

        player = GameObject.Find("Arkanos");
    }

    public void SaveGame()
    {
        SaveSystem.SaveGameData(GameManager.instance.GetGameData());
    }

    public void DisplayStatus()
    {
        int level = player.GetComponent<PlayerBattle>().level;
        lvlValue.text = level.ToString();
        xp.text = player.GetComponent<PlayerBattle>().experience.ToString();
        xpForNextLvl.text = player.GetComponent<PlayerBattle>().XpForLevel().ToString();
        CPValue.text = player.GetComponent<PlayerBattle>().combo.ToString();
        HPValue.text = player.GetComponent<PlayerBattle>().maxHealth.ToString();
        AttValue.text = player.GetComponent<PlayerBattle>().attack.ToString();
        DefValue.text = player.GetComponent<PlayerBattle>().defense.ToString();
        AccValue.text = (player.GetComponent<PlayerBattle>().accuracy * 100).ToString() + '%';
        EvaValue.text = (player.GetComponent<PlayerBattle>().evasion * 100).ToString() + '%';
    }

    public void DisplaySpells()
    {
        SkillAction[] skills = SkillList.GetInstance().GetActions();
        if (skills == null) return;
        int y = 0;
        float SlotCellSize = 30f;

        foreach (SkillAction skill in skills) 
        {
            if (player.GetComponent<PlayerBattle>().CanUseSkill(skill))
            {
                if (y==0)
                {
                    description.text = skill.Description;
                }
                RectTransform skillSlotRectTransform = Instantiate(spellsSlotTemplate, spellsGroup).GetComponent<RectTransform>();
                skillSlotRectTransform.gameObject.SetActive(true);
                skillSlotRectTransform.anchoredPosition = new Vector2(-467, 90 - y * SlotCellSize);
                RectTransform skillQtyRectTransform = Instantiate(spellsQuantityTemplate, spellsGroup).GetComponent<RectTransform>();
                skillQtyRectTransform.gameObject.SetActive(true);
                skillQtyRectTransform.anchoredPosition = new Vector2(-36, 90 - y * SlotCellSize);

                skillSlotRectTransform.gameObject.GetComponent<TextMeshProUGUI>().text = skill.Name;
                string skillQuantity = player.GetComponent<PlayerBattle>().GetSkillAmount(skill).ToString();
                skillQtyRectTransform.gameObject.GetComponent<TextMeshProUGUI>().text = skillQuantity;
                y++;
            }            
        }
    }

    public void ClearSpells()
    {
        if (itemGroup != null)
        {
            foreach (Transform child in spellsGroup)
            {
                if (child == spellsSlotTemplate | child == spellsQuantityTemplate)
                {
                    continue;
                }
                Destroy(child.gameObject);
            }
        }            
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
                    description.text = item.description;
                }

                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemGroup).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.anchoredPosition = new Vector2(128, -92 - y * itemSlotCellSize);
                RectTransform itemQtyRectTransform = Instantiate(itemQuantityTemplate, itemGroup).GetComponent<RectTransform>();
                itemQtyRectTransform.gameObject.SetActive(true);
                itemQtyRectTransform.anchoredPosition = new Vector2(560, -92 - y * itemSlotCellSize);
                y++;
                itemSlotRectTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName;
                itemQtyRectTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = itemAmount.ToString();
            }
        }
    }
    public void ClearItems()
    {
        if (itemGroup != null)
        {
            foreach (Transform child in itemGroup)
            {
                if (child == itemSlotTemplate | child == itemQuantityTemplate)
                {
                    continue;
                }
                Destroy(child.gameObject);
            }
        }
    }
}
