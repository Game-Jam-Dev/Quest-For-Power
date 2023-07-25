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

    public bool EmptyAction(CharacterInfo self, CharacterInfo target)
    {
        Debug.Log("This action is null");
        return false;
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

    private bool HitCheck(CharacterInfo self, CharacterInfo target, int i)
    {
        return Random.Range(0,100) < (self.accuracy - target.evasion + i);
    }

    private void Attack(CharacterInfo self, CharacterInfo target, float d)
    {
        int damage = (int)(self.attack * d - target.defense);
        if (damage < 0) damage = 0;

        damage = SkillCheck(self, target, damage);

        LifeCheck(target, damage);
    }

    public bool LightAttack(CharacterInfo self, CharacterInfo target)
    {
        if (!HitCheck(self, target, 100)) return false;

        else {
            Attack(self, target, 1);
            return true;
        }
    }

    public bool MediumAttack(CharacterInfo self, CharacterInfo target)
    {
        if (!HitCheck(self, target, 80)) return false;

        else {
            Attack(self, target, 1.5f);
            return true;
        }
    }

    public bool HeavyAttack(CharacterInfo self, CharacterInfo target)
    {
        if (!HitCheck(self, target, 60)) return false;

        else {
            Attack(self, target, 2);
            return true;
        }
    }
}