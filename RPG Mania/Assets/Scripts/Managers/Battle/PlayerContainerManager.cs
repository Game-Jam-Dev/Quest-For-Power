using UnityEngine;

public class PlayerContainerManager : MonoBehaviour {
    [SerializeField] private ElementManager element;
    [SerializeField] private ComboManager comboManager;
    [SerializeField] private PlayerHealthManager playerHealthManager;

    private PlayerBattle player;

    public void SetPlayer(PlayerBattle player)
    {
        this.player = player;

        element.SetElement(this.player.element);
        comboManager.SetComboLength(this.player.combo);
        playerHealthManager.SetHealth(this.player);
    }

    public void UpdateComboPoints()
    {
        comboManager.SetComboLength(this.player.combo);
    }

    public void UpdateElement(SkillAction skill)
    {
        element.SetElement(skill);
    }

    public void UpdateElement(ElementManager.Element element)
    {
        this.element.SetElement(element);
    }

    public void UpdateHealth()
    {
        playerHealthManager.UpdateHealth(player.health);
    }

    public bool CanUseCombo(int count)
    {
        return comboManager.CanUseCombo(count);
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