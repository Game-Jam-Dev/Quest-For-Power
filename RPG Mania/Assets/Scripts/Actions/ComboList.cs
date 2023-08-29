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

    public bool EmptyAction(CharacterInfo self, CharacterInfo target, int comboDepth)
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

    private void DamageCheck(CharacterInfo target, int damage)
    {

        damage = CritMultiply(2, 2, damage);

        Debug.Log(damage);

        target.health -= damage;
        if (target.health < 0) target.health = 0;
    }

    private int CritMultiply(int criticalChance, float damageBoost, int damage)
    {

        if (Random.Range(0, 100) < criticalChance) return (int)(damageBoost * damage);

        else return damage;
    }

    private bool HitCheck(CharacterInfo self, CharacterInfo target, float moveAccuracy, int comboDepth)
    {
        float r = Random.Range(0,1f);
        float a  = (moveAccuracy * self.accuracy) - target.evasion + (comboDepth * .05f);
        self.hitTarget = r < a;

        return self.hitTarget;
    }

    private void Attack(CharacterInfo self, CharacterInfo target, float d)
    {
        target.SetAnimationTrigger("Attacked");

        int damage = (int)(self.attack * d - target.defense);
        if (damage < 0) damage = 0;

        damage = SkillCheck(self, target, damage);

        DamageCheck(target, damage);
    }

    public bool LightAttack(CharacterInfo self, CharacterInfo target, int comboDepth)
    {
        string attackName = "Light Attack";
        float accuracy = 1f;
        float damageMultiplier = 1;
        
        return DoAttack(self, target, comboDepth, accuracy, damageMultiplier, attackName);
    }

    public bool MediumAttack(CharacterInfo self, CharacterInfo target, int comboDepth)
    {
        string attackName = "Medium Attack";
        float accuracy = .9f;
        float damageMultiplier = 2f;

        return DoAttack(self, target, comboDepth, accuracy, damageMultiplier, attackName);
    }

    public bool HeavyAttack(CharacterInfo self, CharacterInfo target, int comboDepth)
    {
        string attackName = "Heavy Attack";
        float accuracy = .8f;
        float damageMultiplier = 3f;

        return DoAttack(self, target, comboDepth, accuracy, damageMultiplier, attackName);
    }

    private bool DoAttack(CharacterInfo self, CharacterInfo target, int comboDepth, float accuracy, float damageMultiplier, string attackName)
    {
        self.SetAnimationTrigger(attackName);

        if (!HitCheck(self, target, accuracy, comboDepth)) return false;

        else {
            Attack(self, target, damageMultiplier);
            return true;
        }
    }
}