using UnityEngine;
using System.Collections.Generic;

public class CharacterInfo : MonoBehaviour {
    public string characterName = "";
    public int maxHealth;
    public int health;
    public int stamina;
    public SkillList.Element element;
    public int attack;
    public int defense;
    public int accuracy;
    public int evasion;
    public int combo;
    public SkillAction activeSkill;
    protected List<string> comboKeys = new List<string>();
    protected List<string> skillKeys = new List<string>();

    protected List<ComboAction> comboActions = new List<ComboAction>();
    protected List<SkillAction> skillActions = new List<SkillAction>();

    protected virtual void Start() {
        health = maxHealth;

        comboKeys.Add("light");
        comboKeys.Add("medium");
        comboKeys.Add("heavy");

        comboActions.Add(ComboList.GetInstance().GetAction(comboKeys[0]));
        comboActions.Add(ComboList.GetInstance().GetAction(comboKeys[1]));
        comboActions.Add(ComboList.GetInstance().GetAction(comboKeys[2]));
    }

    public ComboAction GetAction(int i)
    {
        if (i < comboActions.Count) return comboActions[i];

        else return comboActions[0];
    }
    public SkillAction GetSkill(int i)
    {
        if (i < skillActions.Count) return skillActions[i];

        else return skillActions[0];
    }

    public int CountActions()
    {
        return comboActions.Count;
    }
    public int CountSkills()
    {
        return skillActions.Count;
    }

    public void DoAction(ComboAction action, CharacterInfo target)
    {
        action.Action(this, target);
    }

    public void UseSkill(SkillAction skill)
    {
        activeSkill = skill;
        stamina -= skill.Cost;
    }
}