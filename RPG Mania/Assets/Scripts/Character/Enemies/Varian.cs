using UnityEngine;

public class Varian : EnemyBattle {
    private PlayerBattle playerInfo;

    protected override void Start()
    {
        base.Start();

        playerInfo = player.GetComponent<PlayerBattle>();

        skillKeys.Add("water");
        skillKeys.Add("fire");
        skillKeys.Add("wind");
        skillKeys.Add("earth");

        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[0]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[1]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[2]), 0));
        skillActions.Add((SkillList.GetInstance().GetAction(skillKeys[3]), 0));
    }

    protected override void SetStats()
    {
        maxHealth = 12 + level * 4;
        attack = 8 + (int)(level * 1.3f);
        defense = 6 + (int)(level * 1.2f);
        accuracy = .85f + level / 2000f;
        evasion = .02f + level / 1500f;

        combo = 2 + (int)Mathf.Pow(level, .2f);

        health = maxHealth;
    }

    public override void SelectSkill(SkillAction skill)
    {
        activeSkill = skill;
        
        if (activeSkill == SkillList.GetInstance().GetAction("water"))
        {
            element = ElementManager.Element.Water;
        } else if (activeSkill == SkillList.GetInstance().GetAction("fire"))
        {
            element = ElementManager.Element.Fire;
        } else if (activeSkill == SkillList.GetInstance().GetAction("wind"))
        {
            element = ElementManager.Element.Air;
        } else if (activeSkill == SkillList.GetInstance().GetAction("earth"))
        {
            element = ElementManager.Element.Earth;
        }
    }

    public override ComboAction PickEnemyCombo(int currentComboLength)
    {
        // if (currentComboLength == 0 && playerInfo.element != SkillList.Element.None)
        // {
        //     switch (playerInfo.element)
        //     {
        //         case SkillList.Element.Water:
        //         UseSkill(skillActions[3].Item1);
        //         break;
        //         case SkillList.Element.Fire:
        //         UseSkill(skillActions[0].Item1);
        //         break;
        //         case SkillList.Element.Wind:
        //         UseSkill(skillActions[1].Item1);
        //         break;
        //         case SkillList.Element.Earth:
        //         UseSkill(skillActions[2].Item1);
        //         break;
        //     }
        // }

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