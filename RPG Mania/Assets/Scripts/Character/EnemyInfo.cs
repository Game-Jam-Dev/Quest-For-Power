using UnityEngine;

public class EnemyInfo : CharacterInfo {
    public int level;
    public override ComboAction PickEnemyCombo(int currentComboLength)
    {
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

    public int XPFromKill(int playerLevel)
    {
        int xp = 2;

        int levelBonus = (level - playerLevel) * 2;

        return Mathf.Max(xp + levelBonus, 0);
    }
}