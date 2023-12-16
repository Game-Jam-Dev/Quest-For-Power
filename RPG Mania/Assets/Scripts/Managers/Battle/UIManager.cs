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
    [SerializeField] private SkillContainerManager skillContainerManager;
    [SerializeField] private ItemContainerManager itemContainerManager;
    [SerializeField] private GameObject initialContainer, comboContainer, skillContainer, itemContainer, targetContainer;

    public PlayerBattle player;
    public List<EnemyBattle> enemies;

    public EnemyBattle target;

    public void Initialize(PlayerBattle player, List<EnemyBattle> enemies)
    {
        ResetUI();

        // Initialize characters in the fight
        this.player = player;
        this.enemies = enemies;

        playerContainerManager.SetPlayer(player);
        enemyContainerManager.SetEnemies(enemies);
        skillContainerManager.SetSkills(player);
        itemContainerManager.SetItems();
    }

    private void ResetUI()
    {
        initialContainer.SetActive(false);
        comboContainer.SetActive(false);
        skillContainer.SetActive(false);
        itemContainer.SetActive(false);
        targetContainer.SetActive(false);
    }

    public void StartPlayerTurn()
    {
        initialContainer.SetActive(true);
    }

    public void SelectAttack()
    {
        Utility.SwitchActiveObjects(initialContainer, comboContainer);


    }

    public void SelectSkill()
    {
        Utility.SwitchActiveObjects(initialContainer, skillContainer);


    }

    public void SelectItem()
    {
        Utility.SwitchActiveObjects(initialContainer, skillContainer);


    }

    public void SelectEscape()
    {
        
    }

    public void PickCombo(ComboAction combo)
    {
        playerContainerManager.UseCombo(combo.Cost);

    }

    public void PickSkill(SkillAction skill)
    {
        Utility.SwitchActiveObjects(skillContainer, initialContainer);


    }

    public void PickTarget(EnemyBattle enemy)
    {

    }

    public void PickItem()
    {

    }



    public void UpdatePlayerHealth()
    {
        
    }

    public void UpdateEnemyHealth(EnemyBattle enemy)
    {
        enemyContainerManager.UpdateDamage(enemy);
    }
}