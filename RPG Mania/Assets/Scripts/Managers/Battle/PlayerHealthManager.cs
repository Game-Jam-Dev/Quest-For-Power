using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerHealthManager : MonoBehaviour {
    [SerializeField] private Image healthBar;
    private int maxHealth;

    private void Start() {
        healthBar.type = Image.Type.Filled;
        healthBar.fillMethod = Image.FillMethod.Horizontal;
        healthBar.fillAmount = 1;
    }
    public void SetHealth(PlayerBattle player)
    {
        maxHealth = player.maxHealth;
    }

    public void UpdateHealth(int health)
    {
        healthBar.fillAmount = (float)health / maxHealth;
    }

    public void ResetHealth()
    {
        healthBar.fillAmount = 1;
    }
}