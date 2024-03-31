using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using static UnityEditor.Progress;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI itemDescription;
    [SerializeField]
    TextMeshProUGUI skillDescription;

    [SerializeField]
    TextMeshProUGUI lvlValue, xp, xpForNextLvl, CPValue, HPValue, AttValue, DefValue,
        AccValue, EvaValue;


    Transform itemContainer;
    Transform itemSlotTemplate;
    Transform itemQuantityTemplate;

    Transform spellsList;
    Transform spellsContainer;
    Transform spellsSlotTemplate;
    Transform spellsQuantityTemplate;

    GameObject player;

    private void Start()
    {
        itemContainer = transform.Find("Items Container");
        itemSlotTemplate = itemContainer.Find("ItemSlotText");
        itemQuantityTemplate = itemContainer.Find("ItemSlotQuantityTemplate");

        spellsList = transform.Find("Spells Container");
        spellsContainer = spellsList.Find("Spells");
        spellsSlotTemplate = spellsContainer.Find("SpellSlotText");
        spellsQuantityTemplate = spellsContainer.Find("SpellSlotQuantityTemplate");

        player = GameObject.Find("Arkanos");
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
                    skillDescription.text = skill.Description;
                }
                RectTransform skillSlotRectTransform = Instantiate(spellsSlotTemplate, spellsContainer).GetComponent<RectTransform>();
                skillSlotRectTransform.gameObject.SetActive(true);
                skillSlotRectTransform.anchoredPosition = new Vector2(-27, -30 - y * SlotCellSize);
                RectTransform skillQtyRectTransform = Instantiate(spellsQuantityTemplate, spellsContainer).GetComponent<RectTransform>();
                skillQtyRectTransform.gameObject.SetActive(true);
                skillQtyRectTransform.anchoredPosition = new Vector2(76, -53 - y * SlotCellSize);

                skillSlotRectTransform.gameObject.GetComponent<TextMeshProUGUI>().text = skill.Name;
                string skillQuantity = player.GetComponent<PlayerBattle>().GetSkillAmount(skill).ToString();
                skillQtyRectTransform.gameObject.GetComponent<TextMeshProUGUI>().text = skillQuantity;
                y++;
            }            
        }
    }

    public void ClearSpells()
    {
        foreach (Transform child in spellsContainer)
        {
            if (child == spellsSlotTemplate | child == spellsQuantityTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
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
                    itemDescription.text = item.description;
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
