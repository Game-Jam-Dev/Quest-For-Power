using System.Collections.Generic;
using UnityEngine;

public class ComboList
{
    private static ComboList instance;
    private IDictionary<string, ComboAction> actionList;

    private ComboList()
    {
        FillDictionary();
    }

    public static ComboList GetInstance()
    {
        instance ??= new ComboList();

        return instance;
    }

    public ComboAction GetAction(string key)
    {
        if (actionList.ContainsKey(key)) return actionList[key];

        else return new ComboAction("Null", "", 0, EmptyAction);
    }


    private void FillDictionary()
    {
        actionList = new Dictionary<string, ComboAction>()
        {
            {"light", new ComboAction("Light Attack", "light_attack", 1, LightAttack)},
            {"medium", new ComboAction("Medium Attack", "medium_attack", 2, MediumAttack)},
            {"heavy", new ComboAction("Heavy Attack", "heavy_attack", 3, HeavyAttack)},
        };
    }

    public bool EmptyAction(CharacterBattle self, CharacterBattle target, int comboDepth)
    {
        Debug.Log("This action is null");
        return false;
    }

    public bool LightAttack(CharacterBattle self, CharacterBattle target, int comboDepth)
    {
        string attackName = "Light Attack";
        float accuracy =.95f;
        float damageMultiplier = 1;
        
        return ComboAction.DoAttack(self, target, comboDepth, attackName, accuracy, damageMultiplier);
    }

    public bool MediumAttack(CharacterBattle self, CharacterBattle target, int comboDepth)
    {
        string attackName = "Medium Attack";
        float accuracy = .81f;
        float damageMultiplier = 2.5f;

        return ComboAction.DoAttack(self, target, comboDepth, attackName, accuracy, damageMultiplier);
    }

    public bool HeavyAttack(CharacterBattle self, CharacterBattle target, int comboDepth)
    {
        string attackName = "Heavy Attack";
        float accuracy = .7f;
        float damageMultiplier = 4.5f;

        return ComboAction.DoAttack(self, target, comboDepth, attackName, accuracy, damageMultiplier);
    }
}