using UnityEngine;
using System.Collections.Generic;

public class CharacterInfo : MonoBehaviour {
    public string characterName = "";
    public int maxHealth;
    public int health;
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
    protected List<(SkillAction, int)> skillActions = new List<(SkillAction, int)>();

    [SerializeField] private CharacterAnimation ca;

    protected virtual void Start() {
        health = maxHealth;

        comboKeys.Add("light");
        comboKeys.Add("medium");
        comboKeys.Add("heavy");

        comboActions.Add(ComboList.GetInstance().GetAction(comboKeys[0]));
        comboActions.Add(ComboList.GetInstance().GetAction(comboKeys[1]));
        comboActions.Add(ComboList.GetInstance().GetAction(comboKeys[2]));
    }

    public virtual void PrepareCombat()
    {
        ca.SwitchToCombat();
    }

    public void EndCombat()
    {
        ca.SwitchFromCombat();
    }

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

    public Animator GetAnimator() {return ca.GetAnimator();}

    public bool GetIsAttacking() {return ca.isAttacking;}

    public void SetUpTrigger(string triggerName)
    {
        ca.SetUpTrigger(triggerName);
    }

    public virtual ComboAction PickEnemyCombo(int currentComboLength){ return GetCombo(0); }
}