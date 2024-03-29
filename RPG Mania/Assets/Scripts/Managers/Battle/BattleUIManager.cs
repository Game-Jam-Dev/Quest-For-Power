using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour {
    [SerializeField] private BattleManager battleManager;

    [Header("Containers")]
    [SerializeField] private PlayerContainerManager playerContainerManager;
    [SerializeField] private EnemyContainerManager enemyContainerManager;
    [SerializeField] private SkillContainerManager skillContainerManager;
    [SerializeField] private ItemContainerManager itemContainerManager;
    [SerializeField] private LogManager logManager;
    [SerializeField] private GameObject initialContainer, targetContainer, comboContainer, skillContainer, itemContainer;

    public PlayerBattle player;
    public List<EnemyBattle> enemies;

    public EnemyBattle target;
    public Item selectedItem;
    public List<ComboAction> playerCombo = new();

    private bool absorb = false;
    private int absorbCounter = 0;
    private int absorbCounterMax = 3;

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
        targetContainer.SetActive(false);
        comboContainer.SetActive(false);
        skillContainer.SetActive(false);
        itemContainer.SetActive(false);

        target = null;
        selectedItem = null;
        playerCombo.Clear();
        logManager.ClearLog();

        absorbCounter = 0;
    }

    public void StartPlayerTurn()
    {
        initialContainer.SetActive(true);
        targetContainer.SetActive(false);
        comboContainer.SetActive(false);
        skillContainer.SetActive(false);
        itemContainer.SetActive(false);
    }

    public void SelectAttack()
    {
        Utility.SwitchActiveObjects(initialContainer, targetContainer);
        enemyContainerManager.TargetEnemies();
    }

    public void SelectSkill()
    {
        Utility.SwitchActiveObjects(initialContainer, skillContainer);
    }

    public void SelectItem()
    {
        Utility.SwitchActiveObjects(initialContainer, itemContainer);
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

        SetText("Activated " + skill.Name);
    }

    public void PickAbsorb()
    {
        if (absorbCounter > absorbCounterMax) 
        {
            SetText("You can't absorb from these enemies anymore.");
            return;
        }

        absorb = true;
        enemyContainerManager.TargetEnemies();
        Utility.SwitchActiveObjects(skillContainer, targetContainer);
    }

    public void PickTarget(EnemyBattle enemy)
    {
        target = enemy;
        enemyContainerManager.UntargetEnemies();
        targetContainer.SetActive(false);

        if (selectedItem != null)
        {
            SendItemAction(enemy);
        }
        else if (absorb)
        {
            SendAbsorbAction(enemy);
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
            targetContainer.SetActive(true);
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
        targetContainer.SetActive(false);

        if (selectedItem != null)
        {
            selectedItem = null;
        }
        else if (absorb)
        {
            absorb = false;
        }
        else
        {
            playerContainerManager.ResetCombo();
        }

        initialContainer.SetActive(true);
    }

    public void BackFromCombo()
    {
        Utility.SwitchActiveObjects(comboContainer, initialContainer);
        
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
        selectedItem = null;
        EndTurn();
    }

    private void SendAbsorbAction(EnemyBattle target)
    {
        absorbCounter++;
        absorb = false;
        battleManager.SetAbsorbAction(target);
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
        playerContainerManager.UpdateHealth();
    }

    public void UpdateEnemyHealth(EnemyBattle enemy)
    {
        enemyContainerManager.UpdateDamage(enemy);
    }

    public void UpdateSkills()
    {
        skillContainerManager.UpdateSkills(player);
    }

    public void UpdateItems()
    {
        itemContainerManager.UpdateItems();
    }

    public void SetText(string text)
    {
        logManager.SetText(text);
    }
}