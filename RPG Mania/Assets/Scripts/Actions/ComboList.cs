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

        else return new ComboAction("Null", 0, EmptyAction);
    }


    private void FillDictionary()
    {
        actionList = new Dictionary<string, ComboAction>()
        {
            {"light", new ComboAction("Light Attack", 1, LightAttack)},
            {"medium", new ComboAction("Medium Attack", 2, MediumAttack)},
            {"heavy", new ComboAction("Heavy Attack", 3, HeavyAttack)},
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
        float accuracy = 1f;
        float damageMultiplier = 1;
        
        return ComboAction.DoAttack(self, target, comboDepth, attackName, accuracy, damageMultiplier);
    }

    public bool MediumAttack(CharacterBattle self, CharacterBattle target, int comboDepth)
    {
        string attackName = "Medium Attack";
        float accuracy = .9f;
        float damageMultiplier = 1.5f;

        return ComboAction.DoAttack(self, target, comboDepth, attackName, accuracy, damageMultiplier);
    }

    public bool HeavyAttack(CharacterBattle self, CharacterBattle target, int comboDepth)
    {
        string attackName = "Heavy Attack";
        float accuracy = .8f;
        float damageMultiplier = 2f;

        return ComboAction.DoAttack(self, target, comboDepth, attackName, accuracy, damageMultiplier);
    }
}