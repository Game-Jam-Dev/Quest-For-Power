using UnityEngine;

public class Monster : EnemyBattle {
    protected override void Start()
    {
        base.Start();

        AssignElement();
    }

    protected override void SetStats()
    {
        maxHealth = 18 + (int)(level * 2.85f);
        attack = 8 + (int)(level * .75f);
        defense = 8 + (int)(level * .6f);
        accuracy = .4f + level / 3500f;
        evasion = .005f + level / 10000f;

        combo = 3 + (int)Mathf.Pow(level, .25f);

        health = maxHealth;
    }

    public override int XPFromKill(int playerLevel)
    {
        int xp = 5 + (int)Mathf.Log(playerLevel);

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