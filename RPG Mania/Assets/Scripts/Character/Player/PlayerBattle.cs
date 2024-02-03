using UnityEngine;
using System.Collections.Generic;

public class PlayerBattle : CharacterBattle {
    private int experience = 0;
    public int level {get; private set;}

    [SerializeField] private PlayerAnimation pa;

    [SerializeField] private AudioClip waterClip, fireClip, windClip, earthClip, drainClip;
    private List<(AudioClip, float)> elementClips;
    protected override void Start() {
        base.Start();

        skillKeys.Add("health drain");
        skillKeys.Add("water");
        skillKeys.Add("fire");
        skillKeys.Add("wind");
        skillKeys.Add("earth");

        List<int> playerSkillUses = GameManager.instance.GetPlayerSkillUses();
        SkillList skillList = SkillList.GetInstance();

        for (int i = 0; i < skillKeys.Count; i++)
        {
            skillActions.Add((skillList.GetAction(skillKeys[i]), playerSkillUses[i]));
        }

        elementClips = new List<(AudioClip, float)>(){(drainClip, 2.45f), (waterClip, 1.03f), (fireClip, .7f), (windClip, 2.2f), (earthClip, .93f)};
    }

    public override void PrepareCombat()
    {
        pa.SwitchToCombat();
        GetComponent<PlayerMovement>().enabled = false;
    }

    public void WinBattle(int xp, int kills, List<Item> itemDrops)
    {
        int xpForLevel = 10 + (int)Mathf.Pow(level + 1, 2.5f);

        experience += xp;

        if (experience >= xpForLevel) LevelUp(xpForLevel);

        GameManager.instance.SetPlayerExperience(experience);

        GameManager.instance.AddItems(itemDrops);

        SaveSystem.SaveGameData(GameManager.instance.GetGameData());
    }

    private void LevelUp(int xpForLevel)
    {
        experience -= xpForLevel;
        level++;

        SetStats(level);

        GameManager.instance.SetPlayerLevel(level);

        ResetHealth();
    }

    public void SetStats(int level)
    {
        maxHealth = 20 + level * 5;
        attack = 9 + (int)(level * 1.5f);
        defense = 5 + (int)(level * 1.1f);
        accuracy = .85f + level / 2000f;
        evasion = .05f + level / 1000f;

        combo = 3 + (int)Mathf.Pow(level, .3f);
    }

    public void EndCombat()
    {
        pa.SwitchFromCombat();

        DeactivateSkill();

        GetComponent<PlayerMovement>().enabled = true;

        SetStats(level);
    }

    public override void SetAnimationTrigger(string triggerName)
    {
        pa.SetUpTrigger(triggerName);
    }

    public void SetData(int level, int experience, List<int> skillUses)
    {
        this.level = level;
        this.experience = experience;
        
        SetStats(level);

        for (int i = 0; i < skillActions.Count; i++)
        {
            skillActions[i] = (skillActions[i].Item1, skillUses[i]);
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public override void SelectSkill(SkillAction skill)
    {
        activeSkill = skill;
        
        if (activeSkill == SkillList.GetInstance().GetAction("water"))
        {
            element = ElementManager.Element.Water;
        } else if (activeSkill == SkillList.GetInstance().GetAction("fire"))
        {
            element = ElementManager.Element.Fire;
        } else if (activeSkill == SkillList.GetInstance().GetAction("wind"))
        {
            element = ElementManager.Element.Wind;
        } else if (activeSkill == SkillList.GetInstance().GetAction("earth"))
        {
            element = ElementManager.Element.Earth;
        } else {
            element = ElementManager.Element.None;
        }

        pa.SetElement(element);

        audioSource.clip = elementClips[(int) element].Item1;
        audioSource.time = elementClips[(int) element].Item2;
        audioSource?.Play();
    }

    public bool CanUseSkill(SkillAction skill)
    {
        int index = skillActions.FindIndex(item => item.Item1 == skill);

        if (index != -1) return GameManager.instance.GetPlayerSkillUses()[index] > 0;

        else return false;
    }

    public int GetSkillAmount(SkillAction skill)
    {
        int index = skillActions.FindIndex(item => item.Item1 == skill);

        return skillActions[index].Item2;
    }

    public void AbsorbSkill(ElementManager.Element e)
    {
        int index = (int)e;
        int spellsTaken = Random.Range(1,4);

        skillActions[index] = (skillActions[index].Item1, skillActions[index].Item2 + spellsTaken);

        audioSource.clip = elementClips[0].Item1;
        audioSource.time = elementClips[0].Item2;
        audioSource.Play();

        GameManager.instance.SetPlayerSkill(index, skillActions[index].Item2);

        Heal(maxHealth/2);
    }

    public void LoseSkillUse(int n = 1)
    {
        int index = skillActions.FindIndex(item => item.Item1 == activeSkill);

        if (index != -1)
        {
            int newCount = skillActions[index].Item2 - n;
            if (newCount < 0) newCount = 0;
            skillActions[index] = (skillActions[index].Item1, newCount);

            GameManager.instance.SetPlayerSkill(index, skillActions[index].Item2);
        }
    }

    public void DeactivateSkill()
    {
        element = ElementManager.Element.None;
        activeSkill = null;
        pa.SetElement(element);
    }

    public override Animator GetAnimator() {return pa.GetAnimator();}

    public PlayerAnimation GetPlayerAnimation() { return pa; }

    public override bool GetIsAttacking() {return pa.isAttacking;}
}