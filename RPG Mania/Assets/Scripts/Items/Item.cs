using UnityEngine;
public abstract class Item : ScriptableObject {
    public string itemName;
    public int value;
    public string description;
    public Target target;


    public enum Target { Single, All, Self }

    public virtual void Use(CharacterBattle character){}

    public virtual void Use(CharacterBattle[] characters){}
}