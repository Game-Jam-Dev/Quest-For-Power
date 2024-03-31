using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionUIScript : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI CharacterNameField;
    [SerializeField]
    TextMeshProUGUI LvlStatisticsField;
    const string lvlPrefix = "Lvl: ";
    const string currentXpPrefix = "Current XP: ";
    const string xpToLvlUpPrefix = "XP needed for lvl up: ";
    string characterName = "Arkanos";
    int lvl = 1;
    int currentXp = 0;
    int xpToLvlUp = 0;



    // Update is called once per frame
    void Update()
    {
        CharacterNameField.text = characterName;
        LvlStatisticsField.text = lvlPrefix + lvl.ToString() + "\n" + currentXpPrefix + currentXp.ToString() + "\n" + xpToLvlUpPrefix + xpToLvlUp.ToString();
    }
}
