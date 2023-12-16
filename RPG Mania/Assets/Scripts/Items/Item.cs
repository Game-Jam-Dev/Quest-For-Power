using UnityEngine;
public abstract class Item : ScriptableObject {
    public string itemName;
    public int value;
    public string description;

    public virtual void Use(CharacterBattle character){}

    public virtual void Use(CharacterBattle[] characters){}
}