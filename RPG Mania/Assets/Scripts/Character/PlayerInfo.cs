using UnityEngine;

public class PlayerInfo : CharacterInfo {
    protected override void Start() {
        base.Start();

        skillKeys.Add("health drain");
        skillKeys.Add("water");
        skillKeys.Add("fire");
        skillKeys.Add("wind");
        skillKeys.Add("earth");

        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[0]));
        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[1]));
        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[2]));
        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[3]));
        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[4]));
    }

    public override void UseSkill(SkillAction skill)
    {
        activeSkill = skill;
        // stamina -= skill.Cost;
        
        if (activeSkill == SkillList.GetInstance().GetAction("water"))
        {
            element = SkillList.Element.Water;
        } else if (activeSkill == SkillList.GetInstance().GetAction("fire"))
        {
            element = SkillList.Element.Fire;
        } else if (activeSkill == SkillList.GetInstance().GetAction("wind"))
        {
            element = SkillList.Element.Wind;
        } else if (activeSkill == SkillList.GetInstance().GetAction("earth"))
        {
            element = SkillList.Element.Earth;
        }
    }
}