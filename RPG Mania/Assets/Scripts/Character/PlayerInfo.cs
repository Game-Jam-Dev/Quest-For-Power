using UnityEngine;

public class PlayerInfo : CharacterInfo {
    protected override void Start() {
        base.Start();

        skillKeys.Add("health drain");

        skillActions.Add(SkillList.GetInstance().GetAction(skillKeys[0]));
    }
}