using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI pHealth, pCombo, pStamina, eHealth;
    [SerializeField] private GameObject eHealthContainer, actionContainer, skillContainer, targetContainer, pickAction;
    [SerializeField] private Button actionButton, skillButton, targetButton, pickSkillButton, attackButton, escapeButton;
    private List<Button> targetButtons = new List<Button>();
    private List<Button> actionButtons = new List<Button>();
    private List<Button> skillButtons = new List<Button>();
    private GameManager gameManager;
    private int worldScene = 1;
    public int killCount = 0;

    private Queue<CharacterInfo> turnOrder = new Queue<CharacterInfo>();
    private bool awaitCommand = false;
    private int comboLength = 0;

    private List<ComboAction> comboActions = new List<ComboAction>();
    private CharacterInfo target;
    private PlayerInfo player;

    private void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameObject.SetActive(false);
    }
    private void OnEnable() {
        player = gameManager.player;
        player.gameObject.GetComponent<PlayerMovement>().enabled = false;
        var characters = new List<CharacterInfo> { player }.Concat(gameManager.enemies);
        turnOrder = new Queue<CharacterInfo>(characters);

        pCombo.text = player.characterName + "'s Combo Length: " + player.combo;

        attackButton.onClick.AddListener(SelectAttack);
        pickSkillButton.onClick.AddListener(SelectSkill);
        escapeButton.onClick.AddListener(Escape);

        SetEnemies();
        SetActions();
        SetSkills();

        targetContainer.SetActive(false);
        skillContainer.SetActive(false);
        actionContainer.SetActive(false);
        pickAction.SetActive(false);

        UpdateCombo();
        UpdateSkills();
        UpdateHealth();

        StartCoroutine(BattleSequence());
    }

    private void SelectAttack()
    {
        pickAction.SetActive(false);
        targetContainer.SetActive(true);
    }

    private void SelectSkill()
    {
        pickAction.SetActive(false);
        skillContainer.SetActive(true);
    }

    private void Escape()
    {
        EndBattle();
    }

    private void PickTarget(CharacterInfo target)
    {
        this.target = target;
        targetContainer.SetActive(false);
        awaitCommand = false;
    }

    private void PickCombo(ComboAction action)
    {
        if (action.Cost <= player.combo - comboLength) 
        {
            comboActions.Add(action);
            comboLength += action.Cost;
        }

        if (comboLength < player.combo) UpdateCombo();

        else awaitCommand = false;
    }

    private void PickSkill(SkillAction skill)
    {
        if (skill.Cost <= player.stamina) player.UseSkill(skill);

        skillContainer.SetActive(false);
        pickAction.SetActive(true);
        pickSkillButton.interactable = false;
        pStamina.text = player.characterName + "'s Stamina: " + player.stamina;
    }

    private void SetEnemies()
    {
        for (int i = 0; i < gameManager.enemies.Count; i++)
        {
            EnemyInfo enemy = gameManager.enemies[i];

            TextMeshProUGUI enemyHealthText = Instantiate(eHealth, eHealthContainer.transform);

            enemyHealthText.rectTransform.anchoredPosition = new Vector3(0, -i * 100);

            Button selectEnemy = Instantiate(targetButton, targetContainer.transform);

            selectEnemy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.characterName;

            selectEnemy.onClick.AddListener(() => PickTarget(enemy));

            targetButtons.Add(selectEnemy);
        }
    }

    private void SetActions()
    {
        for (int i = 0; i < player.CountActions(); i++)
        {
            ComboAction currentAction = player.GetAction(i);

            Button selectAction = Instantiate(actionButton, actionContainer.transform);

            selectAction.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentAction.Name;

            selectAction.onClick.AddListener(() => PickCombo(currentAction));

            actionButtons.Add(selectAction);
        }
    }

    private void SetSkills()
    {
        for (int i = 0; i < player.CountSkills(); i++)
        {
            SkillAction currentSkill = player.GetSkill(i);

            Button selectSkill = Instantiate(skillButton, skillContainer.transform);

            selectSkill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentSkill.Name;

            selectSkill.onClick.AddListener(() => PickSkill(currentSkill));

            skillButtons.Add(selectSkill);
        }

        pStamina.text = player.characterName + "'s Stamina: " + player.stamina;
    }

    private void UpdateHealth()
    {
        pHealth.text = player.characterName + "'s Health: " + player.health;

        for (int i = 0; i < gameManager.enemies.Count; i++)
        {
            eHealthContainer.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = gameManager.enemies[i].characterName + "'s Health: " + gameManager.enemies[i].health;
        }
    }

    private void UpdateCombo()
    {
        for (int i = 0; i < actionButtons.Count; i++) 
        {
            actionButtons[i].interactable = player.GetAction(i).Cost <= player.combo - comboLength;
        }
    }

    private void UpdateSkills()
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].interactable = player.GetSkill(i).Cost <= player.stamina;
        }

        player.activeSkill = null;
    }

    private IEnumerator BattleSequence()
    {
        while (true) {
            if (turnOrder.Count > 0)
            {
                CharacterInfo activeCharacter = turnOrder.Peek();

                if (activeCharacter == player){
                    awaitCommand = true;
                    pickAction.SetActive(true);

                    if (player.stamina <= 0) pickSkillButton.interactable = false;

                    else pickSkillButton.interactable = true;

                    while (awaitCommand)
                    {

                        yield return null;
                    }
                    awaitCommand = true;
                    
                    actionContainer.SetActive(true);
                    
                    while (awaitCommand)
                    {


                        yield return null;
                    }

                    actionContainer.SetActive(false);

                    foreach (ComboAction a in comboActions)
                    {
                        Debug.Log($"{activeCharacter.characterName} used {a.Name} at {target.characterName}");
                        activeCharacter.DoAction(a, target);
                        UpdateHealth();

                        if (target.health <= 0)
                        {
                            killCount++;

                            if (killCount >= gameManager.enemies.Count) EndBattle();

                            int targetIndex = gameManager.enemies.IndexOf(target as EnemyInfo);
                            
                            targetContainer.transform.GetChild(targetIndex).GetComponent<Button>().interactable = false;

                            var newTurnOrder = new Queue<CharacterInfo>(turnOrder.Where(x => x != target));
                            turnOrder = newTurnOrder;
                            
                            break;
                        }

                        yield return new WaitForSeconds(.5f);
                    }

                    player.activeSkill = null;

                } else {
                    comboActions.Add(activeCharacter.GetAction(0));

                    Debug.Log($"{activeCharacter.characterName} used {comboActions[0].Name} at {player.characterName}");
                    activeCharacter.DoAction(comboActions[0], player);
                    UpdateHealth();

                    if (player.health <= 0) EndBattle();
                }

                NextTurn(activeCharacter);

                yield return new WaitForSeconds(.5f);

            }
            
            
        }
        
    }

    private void NextTurn(CharacterInfo activeCharacter)
    {
        UpdateSkills();
        UpdateCombo();

        turnOrder.Dequeue();
        turnOrder.Enqueue(activeCharacter);

        comboActions.Clear();
        comboLength = 0;
    }

    public void EndBattle()
    {
        Debug.Log("End Battle");

        StopAllCoroutines();

        gameManager.enemies.Clear();

        player.gameObject.GetComponent<PlayerMovement>().enabled = true;

        gameObject.SetActive(false);
    }
}