using UnityEngine;

[CreateAssetMenu(fileName = "Essence", menuName = "Items/Essence", order = 0)]
public class Essence : Item {
    public ElementManager.Element element;

    public override void Use(CharacterBattle character)
    {
        character.Attacked(ElementManager.CalculateEffectiveDamage(element, character.element, value));
    }
}