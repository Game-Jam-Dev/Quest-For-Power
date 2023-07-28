using UnityEngine;

public class Soldier : EnemyInfo {
    private Collider col;
    protected override void Start()
    {
        base.Start();

        

        AssignElement();
    }

    public override void PrepareCombat(int l = 1)
    {
        if (col != null) col.enabled = false;

        SetStats(l);
        
        base.PrepareCombat();
    }

    public void SetStats(int level)
    {
        this.level = level;
        
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