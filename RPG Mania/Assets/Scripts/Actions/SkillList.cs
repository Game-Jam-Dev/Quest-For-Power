using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillList
{
    public static SkillList Instance { get; private set; }
    private IDictionary<string, SkillAction> skillList;

    private SkillList()
    {
        FillDictionary();
    }

    public static SkillList GetInstance()
    {
        Instance ??= new SkillList();

        return Instance;
    }

    public SkillAction GetAction(string key)
    {
        if (skillList.ContainsKey(key)) return skillList[key];

        else return new SkillAction("Null", EmptyAction);
    }

    public SkillAction[] GetActions()
    {
        SkillAction[] actions = skillList.Values.ToArray();

        return actions;
    }


    private void FillDictionary()
    {
        skillList = new Dictionary<string, SkillAction>()
        {
            {"health drain", new SkillAction("Health Drain", HealthDrain)},
            {"water", new SkillAction("Water", InfuseWater)},
            {"wind", new SkillAction("Wind", InfuseWind)},
            {"earth", new SkillAction("Earth", InfuseEarth)},
            {"fire", new SkillAction("Fire", InfuseFire)},
        };
    }

    public int EmptyAction(CharacterBattle self, CharacterBattle target, int damage)
    {
        Debug.Log("This action is null");

        return damage;
    }

    public int InfuseWater(CharacterBattle self, CharacterBattle target, int damage)
    {
        if (target.element == ElementManager.Element.Fire) damage += damage/2;

        else if (target.element == ElementManager.Element.Earth) damage -= damage/2;

        return damage;
    }

    public int InfuseWind(CharacterBattle self, CharacterBattle target, int damage)
    {
        if (target.element == ElementManager.Element.Earth) damage += damage/2;

        else if (target.element == ElementManager.Element.Fire) damage -= damage/2;

        return damage;
    }

    public int InfuseEarth(CharacterBattle self, CharacterBattle target, int damage)
    {
        if (target.element == ElementManager.Element.Water) damage += damage/2;

        else if (target.element == ElementManager.Element.Wind) damage -= damage/2;

        return damage;
    }

    public int InfuseFire(CharacterBattle self, CharacterBattle target, int damage)
    {
        if (target.element == ElementManager.Element.Wind) damage += damage/2;

        else if (target.element == ElementManager.Element.Water) damage -= damage/2;

        return damage;
    }

    public int HealthDrain(CharacterBattle self, CharacterBattle target, int damage)
    {
        self.Heal(damage/3);

        return damage;
    }
}