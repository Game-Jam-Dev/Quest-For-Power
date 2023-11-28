using UnityEngine;

public class Soldier : EnemyBattle {
    protected override void Start()
    {
        base.Start();

        AssignElement();
    }

    protected override void SetStats()
    {
        maxHealth = 12 + (int)(level * 2.5f);
        attack = 8 + (int)(level * .9f);
        defense = 6 + (int)(level * .8f);
        accuracy = .65f + level / 2000f;
        evasion = .02f + level / 1500f;

        combo = 2 + (int)Mathf.Pow(level, .2f);

        health = maxHealth;
    }

    public override int XPFromKill(int playerLevel)
    {
        int xp = 5;

        return xp;
    }

    private void AssignElement()
    {
        if (element == SkillList.Element.None)
        {
            element = (SkillList.Element)Random.Range(1, System.Enum.GetValues(typeof(SkillList.Element)).Length);
        }

        ea.AssignElement(element);

        switch (element)
        {
            case SkillList.Element.Water:
            activeSkill = SkillList.GetInstance().GetAction("water");
            break;
            case SkillList.Element.Fire:
            activeSkill = SkillList.GetInstance().GetAction("fire");
            break;
            case SkillList.Element.Wind:
            activeSkill = SkillList.GetInstance().GetAction("wind");
            break;
            case SkillList.Element.Earth:
            activeSkill = SkillList.GetInstance().GetAction("earth");
            break;
        }
    }
}