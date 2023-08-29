using UnityEngine;

public class Monster : EnemyInfo {
    protected override void Start()
    {
        base.Start();

        AssignElement();
    }

    protected override void SetStats()
    {
        maxHealth = 18 + (int)(level * 2.85f);
        attack = 10 + (int)(level * .75f);
        defense = 9 + (int)(level * .6f);
        accuracy = .5f + level / 3500f;
        evasion = .005f + level / 10000f;

        combo = 3 + (int)Mathf.Pow(level, .25f);

        health = maxHealth;
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