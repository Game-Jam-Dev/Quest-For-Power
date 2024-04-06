using UnityEngine;

public class Soldier : EnemyBattle {
    protected override void Start()
    {
        base.Start();

        AssignElement();
    }

    protected override void SetStats()
    {
        maxHealth = 40 + (int)(level * 2.5f);
        attack = 6 + (int)(level * .9f);
        defense = 3 + (int)(level * .8f);
        accuracy = .65f + level / 2000f;
        evasion = .025f + level / 1500f;

        combo = 3 + (int)Mathf.Pow(level, .2f);

        health = maxHealth;
    }

    public override int XPFromKill(int playerLevel)
    {
        int xp = playerLevel * 2;

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