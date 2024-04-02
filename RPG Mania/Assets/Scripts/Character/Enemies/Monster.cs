using UnityEngine;

public class Monster : EnemyBattle {
    protected override void Start()
    {
        base.Start();

        AssignElement();
    }

    protected override void SetStats()
    {
        maxHealth = 48 + (int)(level * 2.85f);
        attack = 10 + (int)(level * .75f);
        defense = 6 + (int)(level * .6f);
        accuracy = .5f + level / 3500f;
        evasion = .005f + level / 10000f;

        combo = 3 + (int)Mathf.Pow(level, .25f);

        health = maxHealth;
    }

    public override int XPFromKill(int playerLevel)
    {
        int xp = playerLevel * 3;

        return xp;
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