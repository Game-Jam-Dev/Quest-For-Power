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
        maxHealth = level * 8;
        health = maxHealth;
        attack = level * 1.6f;
        defense = accuracy = evasion = level * .8f;
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