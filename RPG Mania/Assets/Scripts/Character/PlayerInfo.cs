using UnityEngine;

public class PlayerInfo : CharacterInfo {
    private int experience = 0;
    public int level {get; private set;}
    protected override void Start() {
        base.Start();

        skillKeys.Add("water");
        skillKeys.Add("fire");
        skillKeys.Add("wind");
        skillKeys.Add("earth");
        skillKeys.Add("health drain");

        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[0]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[1]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[2]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[3]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[4]), 0));
    }

    public void WinBattle(int xp, int kills)
    {
        experience += xp;
        if (experience > level * 11) LevelUp();

        health = (maxHealth/10) * kills;
        if (health > maxHealth) health = maxHealth;
    }

    private void LevelUp()
    {
        SetStats(level + 1);

        Debug.Log("Now level " + level);
    }

    public void SetStats(int level)
    {
        this.level = level;
        experience = level * 10;

        maxHealth = level * 20;
        attack = level * 2;
        defense = accuracy = evasion = level;

        combo = (level / 25) + 3;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public override void UseSkill(SkillAction skill)
    {
        activeSkill = skill;
        
        if (activeSkill == SkillList.GetInstance().GetAction("water"))
        {
            element = SkillList.Element.Water;
        } else if (activeSkill == SkillList.GetInstance().GetAction("fire"))
        {
            element = SkillList.Element.Fire;
        } else if (activeSkill == SkillList.GetInstance().GetAction("wind"))
        {
            element = SkillList.Element.Wind;
        } else if (activeSkill == SkillList.GetInstance().GetAction("earth"))
        {
            element = SkillList.Element.Earth;
        }
    }

    public bool CanUseSkill(SkillAction skill)
    {
        int index = skillActions.FindIndex(item => item.Item1 == skill);

        if (index != -1) return skillActions[index].Item2 > 0;

        else return false;
    }

    public int GetSkillAmount(SkillAction skill)
    {
        int index = skillActions.FindIndex(item => item.Item1 == skill);

        return skillActions[index].Item2;
    }

    public void AbsorbSkill(SkillList.Element e, int n = 1)
    {
        switch (e)
        {
            case SkillList.Element.Water:
            skillActions[0] = (skillActions[0].Item1, skillActions[0].Item2 + n);
            break;
            case SkillList.Element.Fire:
            skillActions[1] = (skillActions[1].Item1, skillActions[1].Item2 + n);
            break;
            case SkillList.Element.Wind:
            skillActions[2] = (skillActions[2].Item1, skillActions[2].Item2 + n);
            break;
            case SkillList.Element.Earth:
            skillActions[3] = (skillActions[3].Item1, skillActions[3].Item2 + n);
            break;
        }
    }

    public void LoseSkill(int n = 1)
    {
        if (activeSkill == null) return;

        int index = skillActions.FindIndex(item => item.Item1 == activeSkill);

        if (index != -1)
        {
            int newCount = skillActions[index].Item2 - n;
            if (newCount < 0) newCount = 0;
            skillActions[index] = (skillActions[index].Item1, newCount);
        }

        element = SkillList.Element.None;
        activeSkill = null;
    }

    public void SetAllSkillAmounts(int n)
    {
        for (int i = 0; i < skillActions.Count; i++)
        {
            skillActions[i] = (skillActions[i].Item1, n);
        }
    }
}