using UnityEngine;

public class PlayerContainerManager : MonoBehaviour {
    [SerializeField] private ElementManager element;
    [SerializeField] private ComboManager comboManager;

    private PlayerBattle player;

    public void SetPlayer(PlayerBattle player)
    {
        this.player = player;

        element.SetElement(player.element);
        comboManager.SetComboLength(player.combo);
    }

    public void UpdateElement(SkillAction skill)
    {
        element.SetElement(skill);
    }

    public void UpdateElement(ElementManager.Element element)
    {
        this.element.SetElement(element);
    }

    public bool UseCombo(int count)
    {
        comboManager.SelectCombo(count);

        return comboManager.MaxCombo();
    }

    public void HighlightCombo(int count)
    {
        // comboManager.HighlightCombo(count);
    }

    public void ResetCombo()
    {
        comboManager.ResetCombo();
    }
}