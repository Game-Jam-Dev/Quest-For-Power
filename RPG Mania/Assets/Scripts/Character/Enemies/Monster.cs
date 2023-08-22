using UnityEngine;

public class Monster : EnemyInfo {
    private Collider col;
    protected override void Start()
    {
        base.Start();

        SetStats();
        AssignElement();
    }

    public override void PrepareCombat()
    {
        if (col != null) col.enabled = false;

        SetStats();
    }

    public void SetStats()
    {
        maxHealth = 18 + (int)(level * 2.85f);
        attack = 8 + (int)(level * .65f);
        defense = 3 + (int)(level * .25f);
        accuracy = .5f + (int)(level / 35f);
        evasion = .005f + (int)(level / 10f);

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