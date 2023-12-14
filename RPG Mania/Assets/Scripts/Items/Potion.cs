using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Items/Potion", order = 0)]
public class Potion : Item {
    public override void Use(CharacterBattle character)
    {
        character.Heal(value);
    }
}