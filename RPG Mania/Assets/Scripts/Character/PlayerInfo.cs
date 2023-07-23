using UnityEngine;

public class PlayerInfo : CharacterInfo {
    protected override void Start() {
        base.Start();

        skillKeys.Add("health drain");
        skillKeys.Add("water");
        skillKeys.Add("wind");
        skillKeys.Add("fire");
        skillKeys.Add("earth");

        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[0]));
        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[1]));
        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[2]));
        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[3]));
        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[4]));
    }
}