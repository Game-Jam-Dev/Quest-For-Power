using UnityEngine;
using System.Collections.Generic;

public class CharacterBattle : MonoBehaviour {
    public string characterName = "";
    public int maxHealth;
    public int health;
    public SkillList.Element element;
    public int attack;
    public int defense;
    public float accuracy;
    public float evasion;
    public int combo;
    public bool hitTarget = false;
    public SkillAction activeSkill;
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip attackClip;

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

        audioSource = GetComponent<AudioSource>();
    }

    public virtual void Kill(){}

    public virtual void Defeated(){}

    public virtual void PrepareCombat(){}

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

    public bool DoAction(ComboAction action, CharacterBattle target, int comboDepth)
    {
        return action.Action(this, target, comboDepth);
    }

    public virtual void SelectSkill(SkillAction skill)
    {
        activeSkill = skill;
    }

    public void PlayAttackSound()
    {
        if (hitTarget)
        {
            audioSource.clip = attackClip;
            audioSource.time = 2f;
            audioSource.Play();
        }
    }

    public virtual void Attacked(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
    }

    public virtual void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth) health = maxHealth;
    }

    public virtual void SetAnimationTrigger(string triggerName) {}

    public virtual Animator GetAnimator() {return null;}

    public virtual bool GetIsAttacking() {return false;}

    public virtual void Recover(){}

    public virtual ComboAction PickEnemyCombo(int currentComboLength){ return GetCombo(0); }

    public virtual void AddReinforcement(EnemyBattle e){}
}