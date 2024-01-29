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
    [SerializeField] private GameObject initialContainer, comboContainer, skillContainer, itemContainer;

    public PlayerBattle player;
    public List<EnemyBattle> enemies;

    public EnemyBattle target;
    public Item selectedItem;
    public List<ComboAction> playerCombo = new();

    public void Initialize(PlayerBattle player, List<EnemyBattle> enemies)
    {
        ResetUI();

        // Initialize characters in the fight
        this.player = player;
        this.enemies = enemies;

        playerContainerManager.SetPlayer(player);
        enemyContainerManager.SetEnemies(enemies, this);
        skillContainerManager.UpdateSkills(player);
        itemContainerManager.UpdateItems();
    }

    private void ResetUI()
    {
        initialContainer.SetActive(false);
        comboContainer.SetActive(false);
        skillContainer.SetActive(false);
        itemContainer.SetActive(false);

        target = null;
        selectedItem = null;
        playerCombo.Clear();
    }

    public void StartPlayerTurn()
    {
        initialContainer.SetActive(true);
    }

    public void SelectAttack()
    {
        initialContainer.SetActive(false);
        enemyContainerManager.TargetEnemies();
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
        battleManager.Escape();
    }

    public void PickCombo(string combo)
    {
        ComboAction action = ComboList.GetInstance().GetAction(combo);

        if (!playerContainerManager.CanUseCombo(action.Cost)) return;

        bool max = playerContainerManager.UseCombo(action.Cost);
        playerCombo.Add(action);

        if (max)
        {
            comboContainer.SetActive(false);
            SendComboAction();
        }
    }

    public void PickSkill(SkillAction skill)
    {
        Utility.SwitchActiveObjects(skillContainer, initialContainer);

        battleManager.activeSkillCounter = 0;

        player.SelectSkill(skill);
        skillContainerManager.UpdateSkills(player);
        playerContainerManager.UpdateElement(skill);
    }

    public void PickTarget(EnemyBattle enemy)
    {
        target = enemy;

        if (selectedItem != null)
        {
            SendItemAction(enemy);
        }
        else
        {
            comboContainer.SetActive(true);
        }
    }

    public void PickItem(Item item)
    {
        selectedItem = item;
        itemContainer.SetActive(false);
        if (item.target == Item.Target.Single)
        {
            enemyContainerManager.TargetEnemies();
        }
        else
        {
            SendItemAction();
        }
    }

    public void BackFromSkill()
    {
        Utility.SwitchActiveObjects(skillContainer, initialContainer);
    }

    public void BackFromItem()
    {
        Utility.SwitchActiveObjects(itemContainer, initialContainer);
    }

    public void BackFromTarget()
    {
        enemyContainerManager.UntargetEnemies();
        if (selectedItem != null)
        {
            selectedItem = null;
            itemContainer.SetActive(true);
        }
        else
        {
            initialContainer.SetActive(true);
            playerContainerManager.ResetCombo();
        }
    }

    public void BackFromCombo()
    {
        enemyContainerManager.TargetEnemies();
        comboContainer.SetActive(false);

        initialContainer.SetActive(true);

        playerContainerManager.ResetCombo();
        playerCombo.Clear();
    }

    private void SendComboAction()
    {
        battleManager.SetComboAction(target, playerCombo);
        EndTurn();
    }

    private void SendItemAction(EnemyBattle target = null)
    {
        battleManager.SetItemAction(selectedItem, target);
        EndTurn();
    }

    private void EndTurn()
    {
        enemyContainerManager.UntargetEnemies();
        playerContainerManager.ResetCombo();
    }

    public void EndBattle()
    {
        ResetUI();
    }

    public void DefeatedEnemy(EnemyBattle enemy)
    {
        enemies.Remove(enemy);
    }

    public void DisableActiveSkill()
    {
        playerContainerManager.UpdateElement(ElementManager.Element.None);
    }

    public void UpdatePlayerHealth()
    {
        
    }

    public void UpdateEnemyHealth(EnemyBattle enemy)
    {
        enemyContainerManager.UpdateDamage(enemy);
    }

    public void UpdateSkills()
    {
        skillContainerManager.UpdateSkills(player);
    }

    public void SetText(string text)
    {

    }
}