using UnityEngine;
using System.Collections.Generic;

public class CharacterBattle : MonoBehaviour {
    public string characterName = "";
    public int maxHealth;
    public int health;
    public ElementManager.Element element;
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

    public virtual void AlterStat(StatChanger.Stat stat, float value, bool isPercent)
    {
        switch (stat)
        {
            case StatChanger.Stat.HP:
                if (isPercent)
                {
                    maxHealth += Mathf.RoundToInt(maxHealth * value);
                }
                else
                {
                    maxHealth += Mathf.RoundToInt(value);
                }
                break;
            case StatChanger.Stat.Atk:
                if (isPercent)
                {
                    attack += Mathf.RoundToInt(attack * value);
                }
                else
                {
                    attack += Mathf.RoundToInt(value);
                }
                break;
            case StatChanger.Stat.Def:
                if (isPercent)
                {
                    defense += Mathf.RoundToInt(defense * value);
                }
                else
                {
                    defense += Mathf.RoundToInt(value);
                }
                break;
            case StatChanger.Stat.Acc:
                if (isPercent)
                {
                    accuracy += accuracy * value;
                }
                else
                {
                    accuracy += value;
                }
                break;
            case StatChanger.Stat.Eva:
                if (isPercent)
                {
                    evasion += evasion * value;
                }
                else
                {
                    evasion += value;
                }
                break;
            case StatChanger.Stat.All:
                if (isPercent)
                {
                    maxHealth += Mathf.RoundToInt(maxHealth * value);
                    attack += Mathf.RoundToInt(attack * value);
                    defense += Mathf.RoundToInt(defense * value);
                    accuracy += accuracy * value;
                    evasion += evasion * value;
                }
                else
                {
                    maxHealth += Mathf.RoundToInt(value);
                    attack += Mathf.RoundToInt(value);
                    defense += Mathf.RoundToInt(value);
                    accuracy += value;
                    evasion += value;
                }
                break;
        }
    }

    public virtual void SetAnimationTrigger(string triggerName) {}

    public virtual Animator GetAnimator() {return null;}

    public virtual bool GetIsAttacking() 
    {
        return false;
    }

    ////public virtual void Recover(){}

    public virtual ComboAction PickEnemyCombo(int currentComboLength){ return GetCombo(0); }

    public virtual void AddReinforcement(EnemyBattle e){}
}