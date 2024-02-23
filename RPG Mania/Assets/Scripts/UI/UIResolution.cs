using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIResolution : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI CharacterNameText;
    [SerializeField]
    TextMeshProUGUI XPStatsText;
    [SerializeField] 
    private WorldManager worldManager;
    [SerializeField]
    private PlayerBattle playerBatle;


    const string LvlPrefix = "Lvl - ";
    const string XPPrefix = "Current XP - ";
    const string NextLvlPrefix = "XP ill next lvl - ";
    const string NewLine = "\n";

    public int lvl = 1;
    public int currentXP = 0;
    public int nextLvl = 0;
    public string characterName = "Arkanos";
    PlayerBattle player;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void OnEnable()
    {
        player = worldManager.GetPlayer();
        CharacterNameText.text = characterName;
        lvl = player.level;
        currentXP = player.experience;
        nextLvl = playerBatle.XpForLevel();
        XPStatsText.text = LvlPrefix + lvl.ToString() + NewLine + XPPrefix + currentXP.ToString() + NewLine + NextLvlPrefix + nextLvl.ToString();
    }
}
