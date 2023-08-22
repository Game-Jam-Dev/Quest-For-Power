using UnityEngine;

public class Soldier : EnemyInfo {
    private Collider col;
    protected override void Start()
    {
        base.Start();

        AssignElement();
    }

    public override void PrepareCombat()
    {
        if (col != null) 
        {
            col.enabled = false;
        }
        
        SetStats();
    }

    public void SetStats()
    {
        maxHealth = 10 + (int)(level * 2.5f);
        attack = 6 + (int)(level * .8f);
        defense = 1 + (int)(level * .35f);
        accuracy = .65f + (int)(level / 20f);
        evasion = .02f + (int)(level / 15f);

        combo = 2 + (int)Mathf.Pow(level, .2f);

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