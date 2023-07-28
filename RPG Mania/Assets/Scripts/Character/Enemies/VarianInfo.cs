using UnityEngine;

public class VarianInfo : EnemyInfo {
    private PlayerInfo player;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();

        skillKeys.Add("water");
        skillKeys.Add("fire");
        skillKeys.Add("wind");
        skillKeys.Add("earth");

        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[0]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[1]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[2]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[3]), 0));
    }

    public override void UseSkill(SkillAction skill)
    {
        activeSkill = skill;
        
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

    public override ComboAction PickEnemyCombo(int currentComboLength)
    {
        if (currentComboLength == 0 && player.element != SkillList.Element.None)
        {
            switch (player.element)
            {
                case SkillList.Element.Water:
                UseSkill(skillActions[3].Item1);
                break;
                case SkillList.Element.Fire:
                UseSkill(skillActions[0].Item1);
                break;
                case SkillList.Element.Wind:
                UseSkill(skillActions[1].Item1);
                break;
                case SkillList.Element.Earth:
                UseSkill(skillActions[2].Item1);
                break;
            }
        } 

        switch (combo - currentComboLength)
        {
            case 1:
            return GetCombo(0);
            case 2:
            return GetCombo(Random.Range(0,2));
            default:
            return GetCombo(Random.Range(0,3));
        }
    }
}