using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleUIManager : MonoBehaviour {
    [SerializeField] private BattleManager battleManager;

    [Header("Containers")]
    [SerializeField] private PlayerContainerManager playerContainerManager;
    [SerializeField] private EnemyContainerManager enemyContainerManager;
    [SerializeField] private SkillContainerManager skillContainerManager;
    [SerializeField] private ItemContainerManager itemContainerManager;
    [SerializeField] private LogManager logManager;
    [SerializeField] private GameObject initialContainer, targetContainer, comboContainer, skillContainer, itemContainer, combatResolution, 
        combatDescriptionBox, combatDescriptionTextObj;

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

        GenerateComboUI();

        playerContainerManager.SetPlayer(player);
        enemyContainerManager.SetEnemies(enemies, this);
        skillContainerManager.UpdateSkills(player);
        itemContainerManager.UpdateItems();
    }

    public void GenerateComboUI()
    {
        foreach (EnemyBattle enemy in enemies)
        {
            enemy.GenerateUIQuestionMarkers();
            enemy.GenerateComboWeakness();
            if (enemy.stunned) 
            {
                enemy.ChangeStunCounter();
            }
        }
    }

    public void EnableCombatDescription()
    {
        if (!combatDescriptionBox.activeSelf)
        {
            combatDescriptionBox.SetActive(true);
        }
    }

    public void AddHoverEventByScript(GameObject gameObject, string descriptionText)
    {
        if (gameObject.GetComponent<EventTrigger>() == null)
        {
            gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((SetDescription) => { UpdateDescriptionText(descriptionText); });
        trigger.triggers.Add(entry);
    }

    public void AddExitEventByScript(GameObject gameObject)
    {
        if (gameObject.GetComponent<EventTrigger>() == null)
        {
            gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((SetDescription) => { ClearDescriptionText(); });
        trigger.triggers.Add(entry);
    }


    public void DisableCombatDescription()
    {
        if (combatDescriptionBox.activeSelf)
        {
            combatDescriptionBox.SetActive(false);
        }
    }

    public void UpdateDescriptionText(string descriptionText)
    {
        if (!combatDescriptionBox.activeSelf)
        {
            combatDescriptionBox.SetActive(true);
        }
        combatDescriptionTextObj.GetComponent<TextMeshProUGUI>().text = descriptionText;
    }

    public void UpdateComboDescriptionText(string comboName)
    {
        if (!combatDescriptionBox.activeSelf)
        {
            combatDescriptionBox.SetActive(true);
        }
        if  (comboName == "light")
        {
            string descriptionText = "95% base accuracy, 1x damage";
            combatDescriptionTextObj.GetComponent<TextMeshProUGUI>().text = descriptionText;
        }
        else if (comboName == "medium")
        {
            string descriptionText = "81% base accuracy, 2.5x damage";
            combatDescriptionTextObj.GetComponent<TextMeshProUGUI>().text = descriptionText;
        }
        else if (comboName == "heavy")
        {
            string descriptionText = "70% base accuracy, 4.5x damage";
            combatDescriptionTextObj.GetComponent<TextMeshProUGUI>().text = descriptionText;
        }
    }

    public void ClearDescriptionText()
    {
        if (combatDescriptionBox.activeSelf)
        {
            combatDescriptionBox.SetActive(false);
        }
        combatDescriptionTextObj.GetComponent<TextMeshProUGUI>().text = "...";
    }

    private void ResetUI()
    {
        CloseMenus();
        
        target = null;
        selectedItem = null;
        playerCombo.Clear();
        logManager.ClearLog();

        absorbCounter = 0;
    }

    private void CloseMenus()
    {
        initialContainer.SetActive(false);
        targetContainer.SetActive(false);
        comboContainer.SetActive(false);
        skillContainer.SetActive(false);
        itemContainer.SetActive(false);
        combatResolution.SetActive(false);
    }

    public void StartPlayerTurn()
    {
        initialContainer.SetActive(true);
        targetContainer.SetActive(false);
        comboContainer.SetActive(false);
        skillContainer.SetActive(false);
        itemContainer.SetActive(false);

        GenerateComboUI();
        playerContainerManager.UpdateComboPoints();
    }

    public void StartCombatResolutionUI()
    {
        CloseMenus();

        if (battleManager.worldManager.hasResolutionUI)
        {
            combatResolution.SetActive(true);
            combatResolution.GetComponent<UIResolution>().Initialize(this, battleManager, player);
        }
        else 
        {
            CloseUI();
        }
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
            ClearDescriptionText();
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
        ClearDescriptionText();
    }

    public void PickAbsorb()
    {
        //if (absorbCounter > absorbCounterMax) 
        //{
        //    SetText("You can't absorb from these enemies anymore.");
        //    return;
        //}

        absorb = true;
        enemyContainerManager.TargetEnemies();
        Utility.SwitchActiveObjects(skillContainer, targetContainer);
        ClearDescriptionText() ;
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
            if (enemy.absorbs >= absorbCounterMax)
            {
                SetText("You can't absorb from this enemy anymore.");
                ClearDescriptionText();
                BackFromTarget();
            }
            else if (!enemy.stunned)
            {
                SetText("You can only absorb stunned enemies");
                BackFromTarget();
            }
            else
            {
                SendAbsorbAction(enemy);
            }
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
        ClearDescriptionText();
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
        ClearDescriptionText();
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
        battleManager.SetComboAction(target, playerCombo, target);
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
        //absorbCounter++;
        absorb = false;
        battleManager.SetAbsorbAction(target);
        target.IncreaseAbsorbs();
        EndTurn();
    }

    private void EndTurn()
    {
        enemyContainerManager.UntargetEnemies();
        playerContainerManager.ResetCombo();
        player.ResetExtraComboPoints();
        playerContainerManager.UpdateComboPoints();
    }

    public void EndBattle()
    {
        StartCombatResolutionUI();
    }

    public void CloseUI()
    {
        ResetUI();

        battleManager.EndBattle();

        gameObject.SetActive(false);
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