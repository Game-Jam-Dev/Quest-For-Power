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

    private void AssignElement()
    {
        if (element == ElementManager.Element.None)
        {
            element = (ElementManager.Element)Random.Range(1, System.Enum.GetValues(typeof(ElementManager.Element)).Length);
        }

        ea.AssignElement(element);

        switch (element)
        {
            case ElementManager.Element.Water:
            activeSkill = SkillList.GetInstance().GetAction("water");
            break;
            case ElementManager.Element.Fire:
            activeSkill = SkillList.GetInstance().GetAction("fire");
            break;
            case ElementManager.Element.Wind:
            activeSkill = SkillList.GetInstance().GetAction("wind");
            break;
            case ElementManager.Element.Earth:
            activeSkill = SkillList.GetInstance().GetAction("earth");
            break;
        }
    }
}