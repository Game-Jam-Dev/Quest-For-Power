using UnityEngine;
public abstract class Item : ScriptableObject {
    public string itemName;
    public int value;
    public string description;

    public virtual void Use(CharacterBattle characterBattle){}

    protected enum Stat
    {
        HP,
        Atk,
        Def,
        Acc,
        Eva,
        All,
    }

    protected enum Target
    {
        Self,
        Enemy,
        All,
    }
}