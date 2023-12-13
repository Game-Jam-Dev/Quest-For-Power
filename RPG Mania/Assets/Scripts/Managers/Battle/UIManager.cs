using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private BattleManager battleManager;

    [Header("Containers")]
    [SerializeField] private PlayerContainerManager playerContainerManager;
    [SerializeField] private EnemyContainerManager enemyContainerManager;

    private PlayerBattle player;
    private List<EnemyBattle> enemies;

    public void Initialize(PlayerBattle player, List<EnemyBattle> enemies)
    {
        // Initialize characters in the fight
        this.player = player;
        this.enemies = enemies;

        playerContainerManager.SetPlayer(player);
        enemyContainerManager.SetEnemies(enemies);
    }

    public void StartPlayerTurn()
    {
        
    }

    private void SetEnemies()
    {

    }
}