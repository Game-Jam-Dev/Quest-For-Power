using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI pHealth, pSkill, pElement, pCombo, actionText, eHealthPrefab;
    [SerializeField] private GameObject eHealthContainer, comboContainer, skillContainer, targetContainer, pickAction;
    [SerializeField] private Button pickAttackButton, pickAbsorbButton, pickSkillButton, comboButtonPrefab, skillButtonPrefab, targetButtonPrefab, backButtonPrefab;

    private List<Button> targetButtons = new();
    private List<Button> comboButtons = new();
    private List<Button> skillButtons = new();
    private List<ComboAction> playerCombo = new();
    private int playerComboLength = 0;
    private int absorbCounter = 0;

    [SerializeField] private BattleManager battleManager;
    private List<EnemyInfo> enemies = new();
    private PlayerInfo player;

    private bool canCombo = true;
    private CharacterInfo target;

    public void SetForBattle(PlayerInfo player, List<EnemyInfo> enemies)
    {
        // ensure everything is reset
        ClearUI();

        // Initialize characters in the fight
        this.player = player;
        this.enemies = enemies;

        // Setup all the menus
        SetEnemies();
        SetHealth();
        SetCombos();
        
        // Deactivate menus until they are needed
        targetContainer.SetActive(false);
        skillContainer.SetActive(false);
        comboContainer.SetActive(false);
        pickAction.SetActive(false);
    }

    public void ActivateForPlayerTurn()
    {
        // Update UI
        SetText("");

        // reset used variables
        target = null;
        playerComboLength = 0;
        playerCombo.Clear();
        UpdateCombo();
        UpdateSkills();
        
        pickAction.SetActive(true);
        Utility.SetActiveButton(pickAttackButton);
    }

    private void SetEnemies()
    {
        // Make ui components for each enemy
        for (int i = 0; i < enemies.Count; i++)
        {
            // set health display
            EnemyInfo enemy = enemies[i];
            TextMeshProUGUI enemyHealthText = Instantiate(eHealthPrefab, eHealthContainer.transform);
            enemyHealthText.rectTransform.anchoredPosition = new Vector3(0, -i * 100);

            // set button to select enemy
            Button selectEnemy = Instantiate(targetButtonPrefab, targetContainer.transform);
            selectEnemy.onClick.AddListener(() => PickTarget(enemy));
            targetButtons.Add(selectEnemy);

            // set button text
            string leadingText = "";
            if (enemy.element != SkillList.Element.None) leadingText = enemy.element + " ";
            selectEnemy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = leadingText + enemy.characterName;
        }

        Button back = Instantiate(backButtonPrefab, targetContainer.transform);
        back.onClick.AddListener(BackFromTarget);
    }

    private void SetHealth()
    {
        // update player's health
        UpdatePlayerHealth();

        // update health for each enemy
        UpdateEnemyHealth();
    }

    private void SetCombos()
    {
        // set a button for each combo attack
        for (int i = 0; i < player.CountActions(); i++)
        {
            // get the player's next combo attack
            ComboAction currentCombo = player.GetCombo(i);

            // make the button to select attack
            Button selectCombo = Instantiate(comboButtonPrefab, comboContainer.transform);
            selectCombo.onClick.AddListener(() => PickCombo(currentCombo));
            comboButtons.Add(selectCombo);

            // set button text
            selectCombo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCombo.Name;
        }

        Button back = Instantiate(backButtonPrefab, comboContainer.transform);
        back.onClick.AddListener(BackFromCombo);

        // check if combos can be used
        UpdateCombo();
    }

    private void SetSkills()
    {
        // add button for each skill
        for (int i = 0; i < player.CountSkills(); i++)
        {
            SkillAction currentSkill = player.GetSkill(i);
            // make sure player has enough uses
            if (player.CanUseSkill(currentSkill))
            { 
                // make a button for the skill
                Button selectSkill = Instantiate(skillButtonPrefab, skillContainer.transform);
                selectSkill.onClick.AddListener(() => PickSkill(currentSkill));
                skillButtons.Add(selectSkill);

                // set text for skill
                int skillAmount = player.GetSkillAmount(currentSkill);
                string skillDisplay = currentSkill.Name + " (" + skillAmount + " Use";
                if (skillAmount != 1)
                    skillDisplay += "s";
                skillDisplay += ")";
                selectSkill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = skillDisplay;
            }
        }

        // set it so menu can't be accessed if the player has no skills
        pickSkillButton.interactable = skillContainer.transform.childCount > 0;

        Button back = Instantiate(backButtonPrefab, skillContainer.transform);
        back.onClick.AddListener(BackFromSkill); 
    }

    public void UpdateSkills()
    {
        // should be called whenever anything related to the player's skills changes

        // clear previously displayed skills
        for (int i = 0; i < skillContainer.transform.childCount; i++)
        {
            Destroy(skillContainer.transform.GetChild(i).gameObject);
        }

        skillButtons.Clear();
        
        // re-add skills
        SetSkills();
    }

    public void UpdatePlayerHealth()
    {
        // set player's health ui element
        pHealth.text = player.characterName + "'s Health: " + player.health + " Level " + player.level;
    }

    public void UpdateEnemyHealth()
    {
        // set all enemys' health ui elements
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyInfo enemy = enemies[i];

            // set health display for enemy
            string elementText = "";
            if (enemy.element != SkillList.Element.None) elementText = enemy.element + " ";
            eHealthContainer.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = elementText + enemy.characterName + "'s Health: " + enemy.health;
        }
    }

    public void UpdateCombo()
    {
        // set if button can be selected depending on if the player has enough combo remaining
        for (int i = 0; i < comboButtons.Count; i++) 
        {
            comboButtons[i].interactable = player.GetCombo(i).Cost <= player.combo - playerComboLength;
        }

        // set player's current combo length
        pCombo.text = player.characterName + "'s Combo Length: " + (player.combo - playerComboLength);
    }

    public void SelectAttack()
    {
        pickAction.SetActive(false);
        targetContainer.SetActive(true);
        Utility.SetActiveButton(targetButtons[0]);
    }

    public void SelectAbsorb()
    {
        canCombo = false;
        pickAction.SetActive(false);
        targetContainer.SetActive(true);
        Utility.SetActiveButton(targetButtons[0]);
    }

    public void SelectSkill()
    {
        pickAction.SetActive(false);
        skillContainer.SetActive(true);
        Utility.SetActiveButton(skillButtons[0]);
    }

    public void SelectEscape()
    {
        battleManager.Escape();
    }

    private void BackFromSkill()
    {
        skillContainer.SetActive(false);
        pickAction.SetActive(true);
        Utility.SetActiveButton(pickAttackButton);
    }

    private void BackFromTarget()
    {   
        targetContainer.SetActive(false);
        pickAction.SetActive(true);
        battleManager.UnselectSkill();
        canCombo = true;
        Utility.SetActiveButton(pickAttackButton);
    }

    private void BackFromCombo()
    {
        comboContainer.SetActive(false);

        // reset combo when player backs out
        playerCombo.Clear();
        playerComboLength = 0;
        UpdateCombo();

        targetContainer.SetActive(true);
        Utility.SetActiveButton(targetButtons[0]);
    }

    private void PickTarget(CharacterInfo target)
    {
        this.target = target;

        targetContainer.SetActive(false);

        if (canCombo)
        {
            comboContainer.SetActive(true);
            Utility.SetActiveButton(comboButtons[0]);
        } 
        else 
        {
            SendAbsorbAction();
        }
    }

    private void PickCombo(ComboAction action)
    {
        if (action.Cost <= player.combo - playerComboLength) 
        {
            playerCombo.Add(action);
            playerComboLength += action.Cost;
        }

        if (playerComboLength < player.combo) 
        {
            if (action.Cost > player.combo - playerComboLength)
                Utility.SetActiveButton(comboButtons[0]);
                
            UpdateCombo();
        }
        else
        {
            pCombo.text = player.characterName + "'s Combo Length: " + (player.combo - playerComboLength);
            SendComboAction();
            comboContainer.SetActive(false);
        } 
    }

    private void PickSkill(SkillAction skill)
    {
        // player activates selected skill
        player.SelectSkill(skill);

        // set ui to show skill
        pSkill.text = player.characterName + "'s Active Skill: " + player.activeSkill.Name;
        pElement.text = player.characterName + "'s Active Element: " + player.element;

        // let battlemanager know a skill is picked
        SendSkillAction();

        skillContainer.SetActive(false);
        targetContainer.SetActive(true);
        Utility.SetActiveButton(targetButtons[0]);
    }

    private void SendComboAction()
    {
        battleManager.SetComboAction(target, playerCombo);
    }
    
    private void SendAbsorbAction()
    {
        battleManager.SetAbsorbAction(target);
        
        // manage absorb counter
        absorbCounter++;
        if (absorbCounter >= 3)
            pickAbsorbButton.interactable = false;

        canCombo = true;
    }

    private void SendSkillAction()
    {
        battleManager.activeSkillCounter = 0;
    }

    public void SetText(string s)
    {
        actionText.text = s;
    }

    public void DefeatedEnemy(EnemyInfo enemy)
    {
        // get the enemy's index in enemy list
        int enemyIndex = enemies.IndexOf(enemy);

        // remove button associated with enemy
        Button targetButton = targetContainer.transform.GetChild(enemyIndex).GetComponent<Button>();
        targetButtons.Remove(targetButton);
        targetButton.interactable = false;

        // set enemy's health ui to defeated text
        eHealthContainer.transform.GetChild(enemyIndex).GetComponent<TextMeshProUGUI>().text = enemy.characterName + " is defeated";
    }

    public void ClearActiveSkill()
    {
        pSkill.text = "";
        pElement.text = "";
    }

    public void ClearUI()
    {
        // clear all lists and ui elements from any previous battles
        targetButtons.Clear();
        comboButtons.Clear();
        skillButtons.Clear();

        foreach (TextMeshProUGUI t in eHealthContainer.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < targetContainer.transform.childCount; i++)
        {
            Destroy(targetContainer.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < skillContainer.transform.childCount; i++)
        {
            Destroy(skillContainer.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < comboContainer.transform.childCount; i++)
        {
            Destroy(comboContainer.transform.GetChild(i).gameObject);
        }

        // reset absorb counter
        pickAbsorbButton.interactable = true;
        absorbCounter = 0;

        // reset used variables
        target = null;
        playerComboLength = 0;
        playerCombo.Clear();

        // reset player ui strings
        ClearActiveSkill();
    }
    
}