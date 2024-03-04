using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditorInternal.VersionControl;

public class UIResolution : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI characterNameText;
    [SerializeField]
    TextMeshProUGUI xpStatsText;
    [SerializeField]
    TextMeshProUGUI itemListText; 
    
    private BattleManager battleManager;
    private BattleUIManager battleUIManager;


    const string lvlPrefix = "Lvl - ";
    const string xpPrefix = "Current XP - ";
    const string nextLvlPrefix = "XP for next lvl - ";
    const string newLine = "\n";

    public int lvl = 1;
    public int currentXP = 0;
    public int nextLvl = 0;
    public string characterName = "Arkanos";
    private List<Item> items;
    private string itemsString = "Items:\n";
    PlayerBattle player;
    private Coroutine close;

    public void Initialize(BattleUIManager battleUIManager, BattleManager battleManager, PlayerBattle player)
    {
        this.player = player;
        this.battleManager = battleManager;
        this.battleUIManager = battleUIManager;

        characterNameText.text = characterName;

        lvl = player.level;
        currentXP = player.experience;
        nextLvl = player.XpForLevel();
        xpStatsText.text = lvlPrefix + lvl.ToString() + newLine + xpPrefix + currentXP.ToString() + newLine + nextLvlPrefix + nextLvl.ToString();

        items = battleManager.itemDrops;
        itemsString = "Items:\n";

        foreach (Item item in items)
        {
            itemsString += item.itemName + "\n";
        }

        itemListText.text = itemsString;

        if (close != null) close = null;

        close = StartCoroutine(Close());
    }
    private IEnumerator Close()
    {
        while (Input.GetMouseButtonDown(0) == false)
        {
            yield return null;
        }

        battleUIManager.CloseUI();
    }
}
