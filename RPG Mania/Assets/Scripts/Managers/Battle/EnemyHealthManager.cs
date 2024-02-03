using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour {
    [SerializeField] private GameObject healthBar;
    private List<GameObject> healthPieces = new();
    private int maxHealth, currentHealth;

    public void Initialize(EnemyBattle enemyBattle)
    {
        healthPieces.Clear();
        foreach (Transform child in healthBar.transform)
        {
            healthPieces.Add(child.gameObject);
        }

        healthPieces.Reverse();

        ResetBar();

        maxHealth = enemyBattle.maxHealth;
        currentHealth = maxHealth;
    }

    public void UpdateHealth(int health)
    {
        currentHealth = health;

        UpdateBar();
    }

    private void UpdateBar()
    {
        float divisor = maxHealth / (float)healthPieces.Count;
        float current = maxHealth - currentHealth;

        for (int i = 0; i < healthPieces.Count; i++)
        {
            if (current <= (i+1) * divisor)
            {
                healthPieces[i].SetActive(true);
            }
            else
            {
                healthPieces[i].SetActive(false);
            }
        }
    }

    private void ResetBar()
    {
        foreach (GameObject piece in healthPieces)
        {
            piece.SetActive(true);
        }
    }

    public void Defeated()
    {
        foreach (GameObject piece in healthPieces)
        {
            piece.SetActive(false);
        }
    }
}