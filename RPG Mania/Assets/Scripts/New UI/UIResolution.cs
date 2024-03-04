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
    [SerializeField] 
    private WorldManager worldManager;
    [SerializeField]
    private PlayerBattle playerBatle;
    [SerializeField]
    private BattleManager battleManager;


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

    // Update is called once per frame
    void OnEnable()
    {
        player = worldManager.GetPlayer();
        characterNameText.text = characterName;
        lvl = player.level;
        currentXP = player.experience;
        nextLvl = playerBatle.XpForLevel();
        xpStatsText.text = lvlPrefix + lvl.ToString() + newLine + xpPrefix + currentXP.ToString() + newLine + nextLvlPrefix + nextLvl.ToString();
        items = battleManager.itemDrops;
        foreach (Item item in items)
        {
                itemsString += item.itemName + "\n";
        }

        itemListText.text = itemsString;
    }

    private void OnDisable()
    {
        itemsString = "Items:\n";
    }
}
