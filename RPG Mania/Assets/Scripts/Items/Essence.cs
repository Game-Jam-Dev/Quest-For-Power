using UnityEngine;

[CreateAssetMenu(fileName = "Essence", menuName = "Items/Essence", order = 0)]
public class Essence : Item {
    public ElementManager.Element element;
    public Range range;

    public enum Range { single, all }

    public override void Use(CharacterBattle character)
    {
        character.Attacked(ElementManager.CalculateEffectiveDamage(element, character.element, value));
    }

    public override void Use(CharacterBattle[] characters)
    {
        foreach (CharacterBattle character in characters)
        {
            character.Attacked(ElementManager.CalculateEffectiveDamage(element, character.element, value));
        }
    }
}