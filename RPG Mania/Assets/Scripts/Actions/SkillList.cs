using System.Collections.Generic;
using UnityEngine;

public class SkillList
{
    public static SkillList Instance { get; private set; }
    private IDictionary<string, SkillAction> skillList;

    public enum Element{
        None,
        Water,
        Fire,
        Earth,
        Wind,
    }

    private SkillList()
    {
        FillDictionary();
    }

    public static SkillList GetInstance()
    {
        if (Instance == null)
        {
            Instance = new SkillList();
        }

        return Instance;
    }

    public SkillAction GetAction(string key)
    {
        if (skillList.ContainsKey(key)) return skillList[key];

        else return new SkillAction("Null", 0, EmptyAction);
    }


    private void FillDictionary()
    {
        skillList = new Dictionary<string, SkillAction>()
        {
            {"health drain", new SkillAction("Health Drain", 5, HealthDrain)},
            {"water", new SkillAction("Water", 3, InfuseWater)},
            {"wind", new SkillAction("Wind", 3, InfuseWind)},
            {"earth", new SkillAction("Earth", 3, InfuseEarth)},
            {"fire", new SkillAction("Fire", 3, InfuseFire)},
        };
    }

    public int EmptyAction(CharacterInfo self, CharacterInfo target, int damage)
    {
        Debug.Log("This action is null");

        return damage;
    }

    public int InfuseWater(CharacterInfo self, CharacterInfo target, int damage)
    {
        if (target.element == Element.Fire) damage += damage/2;

        else if (target.element == Element.Earth) damage -= damage/2;

        return damage;
    }

    public int InfuseWind(CharacterInfo self, CharacterInfo target, int damage)
    {
        if (target.element == Element.Earth) damage += damage/2;

        else if (target.element == Element.Fire) damage -= damage/2;

        return damage;
    }

    public int InfuseEarth(CharacterInfo self, CharacterInfo target, int damage)
    {
        if (target.element == Element.Water) damage += damage/2;

        else if (target.element == Element.Wind) damage -= damage/2;

        return damage;
    }

    public int InfuseFire(CharacterInfo self, CharacterInfo target, int damage)
    {
        if (target.element == Element.Wind) damage += damage/2;

        else if (target.element == Element.Water) damage -= damage/2;

        return damage;
    }

    public int HealthDrain(CharacterInfo self, CharacterInfo target, int damage)
    {
        self.health += damage/3;

        if (self.health > self.maxHealth) self.health = self.maxHealth;

        return damage;
    }
}