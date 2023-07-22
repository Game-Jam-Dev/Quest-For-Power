using System.Collections.Generic;
using UnityEngine;

public class ComboList
{
    public static ComboList Instance { get; private set; }
    private IDictionary<string, ComboAction> actionList;

    private ComboList()
    {
        FillDictionary();
    }

    public static ComboList GetInstance()
    {
        if (Instance == null)
        {
            Instance = new ComboList();
        }

        return Instance;
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

    public void EmptyAction(CharacterInfo self, CharacterInfo target)
    {
        Debug.Log("This action is null");
    }

    private int SkillCheck(CharacterInfo self, CharacterInfo target, int damage)
    {
        if (self.activeSkill != null)
            damage = self.activeSkill.Action(self, target, damage);

        return damage;
    }

    private void LifeCheck(CharacterInfo target, int damage)
    {

        if (Random.Range(0, 100) < 5) damage *= 5;

        Debug.Log(damage);

        target.health -= damage;
        if (target.health < 0) target.health = 0;
    }

    public void LightAttack(CharacterInfo self, CharacterInfo target)
    {
        int damage = self.attack - target.defense;
        if (damage < 0) damage = 0;

        damage = SkillCheck(self, target, damage);

        LifeCheck(target, damage);
    }

    public void MediumAttack(CharacterInfo self, CharacterInfo target)
    {
        int damage = 0;

        if (Random.Range(0, 100) < (self.accuracy - target.evasion + 80))
        {
            damage = (int)(self.attack * 2.5) - target.defense;
            if (damage < 0) damage = 0;

        } else {
            Debug.Log("Miss");
        }
        
        damage = SkillCheck(self, target, damage);

        LifeCheck(target, damage);
    }

    public void HeavyAttack(CharacterInfo self, CharacterInfo target)
    {
        int damage = 0;

        if (Random.Range(0, 100) < (self.accuracy - target.evasion + 60))
        {
            damage = self.attack * 5 - target.defense;
            if (damage < 0) damage = 0;

        } else 
        {
            Debug.Log("Miss");
        }

        damage = SkillCheck(self, target, damage);

        LifeCheck(target, damage);
    }
}