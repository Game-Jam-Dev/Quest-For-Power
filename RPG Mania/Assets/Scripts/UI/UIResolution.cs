using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditorInternal.VersionControl;

public class UIResolution : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI CharacterNameText;
    [SerializeField]
    TextMeshProUGUI XPStatsText;
    [SerializeField]
    TextMeshProUGUI ItemListText;
    [SerializeField] 
    private WorldManager worldManager;
    [SerializeField]
    private PlayerBattle playerBatle;
    [SerializeField]
    private BattleManager battleManager;


    const string LvlPrefix = "Lvl - ";
    const string XPPrefix = "Current XP - ";
    const string NextLvlPrefix = "XP for next lvl - ";
    const string NewLine = "\n";

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
        CharacterNameText.text = characterName;
        lvl = player.level;
        currentXP = player.experience;
        nextLvl = playerBatle.XpForLevel();
        XPStatsText.text = LvlPrefix + lvl.ToString() + NewLine + XPPrefix + currentXP.ToString() + NewLine + NextLvlPrefix + nextLvl.ToString();
        items = battleManager.itemDrops;
        foreach (Item item in items)
        {
                itemsString += item.itemName + "\n";
        }

        ItemListText.text = itemsString;
    }

    private void OnDisable()
    {
        itemsString = "Items:\n";
    }
}
