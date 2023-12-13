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

    public void UseCombo(int count)
    {
        comboManager.SelectCombo(count);
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