using UnityEngine;
using System.Collections.Generic;

public class CharacterInfo : MonoBehaviour {
    public string characterName = "";
    public int maxHealth;
    public int health;
    public SkillList.Element element;
    public float attack;
    public float defense;
    public float accuracy;
    public float evasion;
    public int combo;
    public SkillAction activeSkill;
    protected List<string> comboKeys = new List<string>();
    protected List<string> skillKeys = new List<string>();

    protected List<ComboAction> comboActions = new List<ComboAction>();
    protected List<(SkillAction, int)> skillActions = new List<(SkillAction, int)>();

    protected virtual void Start() {
        health = maxHealth;

        comboKeys.Add("light");
        comboKeys.Add("medium");
        comboKeys.Add("heavy");

        comboActions.Add(ComboList.GetInstance().GetAction(comboKeys[0]));
        comboActions.Add(ComboList.GetInstance().GetAction(comboKeys[1]));
        comboActions.Add(ComboList.GetInstance().GetAction(comboKeys[2]));
    }

    public virtual void PrepareCombat(int l = 1) {}

    public ComboAction GetCombo(int i)
    {
        if (i < comboActions.Count) return comboActions[i];

        else return comboActions[0];
    }
    public SkillAction GetSkill(int i)
    {
        if (i < skillActions.Count) return skillActions[i].Item1;

        else return skillActions[0].Item1;
    }

    public int CountActions()
    {
        return comboActions.Count;
    }
    public int CountSkills()
    {
        return skillActions.Count;
    }

    public bool DoAction(ComboAction action, CharacterInfo target, int comboDepth)
    {
        return action.Action(this, target, comboDepth);
    }

    public virtual void UseSkill(SkillAction skill)
    {
        activeSkill = skill;
    }

    public virtual void SetUpTrigger(string triggerName) {}

    public virtual Animator GetAnimator() {return null;}

    public virtual bool GetIsAttacking() {return false;}

    public virtual ComboAction PickEnemyCombo(int currentComboLength){ return GetCombo(0); }
}