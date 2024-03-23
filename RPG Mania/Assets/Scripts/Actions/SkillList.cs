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

        else return new SkillAction("Null", EmptyAction, "Null");
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
            {"water", new SkillAction("Water", InfuseWater, "Do extra damage against fire and less against earth")},
            {"wind", new SkillAction("Wind", InfuseWind, "Do extra damage against earth and less against fire")},
            {"earth", new SkillAction("Earth", InfuseEarth, "Do extra damage against water and less against wind")},
            {"fire", new SkillAction("Fire", InfuseFire, "Do extra damage against wind and less against water")},
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