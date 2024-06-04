using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildsMapsManager : MonoBehaviour
{
    [SerializeField] private Sprite explorationMap;
    [SerializeField] private Sprite combatMap;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeToCombat()
    {
        spriteRenderer.sprite = combatMap;
    }

    public void ChangeToExploration()
    {
        spriteRenderer.sprite = explorationMap;
    }
}
