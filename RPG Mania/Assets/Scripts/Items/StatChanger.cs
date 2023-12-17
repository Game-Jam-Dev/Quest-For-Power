using UnityEngine;

[CreateAssetMenu(fileName = "StatChanger", menuName = "Items/StatChanger", order = 0)]
public class StatChanger : Item {
    public Stat stat;
    public bool isPositive = true;
    public bool isPercent = false;
    
    public enum Stat
    {
        HP,
        Atk,
        Def,
        Acc,
        Eva,
        All,
    }

    
    public override void Use(CharacterBattle character)
    {
        float adjustedValue = isPositive ? value : -value;
        adjustedValue = isPercent ? adjustedValue / 100 : adjustedValue;

        character.AlterStat(stat, adjustedValue, isPercent);
    }
    
}