using UnityEngine;

public class Soldier : EnemyInfo {
    protected override void Start()
    {
        base.Start();

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