using UnityEngine;

public class EnemyInfo : CharacterInfo {
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
}