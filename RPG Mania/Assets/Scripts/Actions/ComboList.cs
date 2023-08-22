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

        target.health -= damage;
        if (target.health < 0) target.health = 0;
    }

    private int CritMultiply(int criticalChance, float damageBoost, int damage)
    {


        if (Random.Range(0, 100) < criticalChance) return (int)(damageBoost * damage);

        else return 1;
    }

    private bool HitCheck(CharacterInfo self, CharacterInfo target, float moveAccuracy, int comboDepth)
    {
        int r = Random.Range(0,100);
        float a  = (moveAccuracy * self.accuracy) - target.evasion + (comboDepth * .05f);
        a *= 10;
        self.hit = r < a;

        Debug.Log(self.accuracy + " " + target.evasion);

        Debug.Log("random: " + r + ". acc calc: " + a);

        return r < a;
    }

    private void Attack(CharacterInfo self, CharacterInfo target, float d)
    {
        target.SetUpTrigger("Attacked");

        int damage = Mathf.RoundToInt(self.attack * d - target.defense);
        if (damage < 0) damage = 0;

        damage = SkillCheck(self, target, damage);

        DamageCheck(target, damage);
    }

    public bool LightAttack(CharacterInfo self, CharacterInfo target, int comboDepth)
    {
        DoAnimation(self, "Light Attack");

        float accuracy = 1f;
        float damageMultiplier = 1;
        
        return DoAttack(self, target, comboDepth, accuracy, damageMultiplier);
    }

    public bool MediumAttack(CharacterInfo self, CharacterInfo target, int comboDepth)
    {
        DoAnimation(self, "Medium Attack");

        float accuracy = .9f;
        float damageMultiplier = 2.5f;

        return DoAttack(self, target, comboDepth, accuracy, damageMultiplier);
    }

    public bool HeavyAttack(CharacterInfo self, CharacterInfo target, int comboDepth)
    {
        DoAnimation(self, "Heavy Attack");

        float accuracy = .8f;
        float damageMultiplier = 4.5f;

        return DoAttack(self, target, comboDepth, accuracy, damageMultiplier);
    }

    private bool DoAttack(CharacterInfo self, CharacterInfo target, int comboDepth, float accuracy, float damageMultiplier)
    {
        if (!HitCheck(self, target, accuracy, comboDepth)) return false;

        else {
            Attack(self, target, damageMultiplier);
            return true;
        }
    }

    private void DoAnimation(CharacterInfo self, string triggerName)
    {
        self.SetUpTrigger(triggerName);
    }
}