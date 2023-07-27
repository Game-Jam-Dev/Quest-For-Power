using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class BattleThroneManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI pHealth, pSkill, pElement, pCombo, eHealth, actionText;
    [SerializeField] private GameObject eHealthContainer, comboContainer, skillContainer, targetContainer, pickAction;
    [SerializeField] private Button comboButton, skillButton, targetButton, attackButton, pickSkillButton, escapeButton, backButton;
    private List<Button> targetButtons = new List<Button>();
    private List<Button> comboButtons = new List<Button>();
    private List<Button> skillButtons = new List<Button>();
    private GameManager gameManager;
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
        
        attackButton.onClick.AddListener(SelectAttack);
        pickSkillButton.onClick.AddListener(SelectSkill);
        escapeButton.onClick.AddListener(SelectEscape);

        SetEnemies();
        SetCombos();

        UpdateCombo();
        UpdateSkills();
        UpdateHealth();

        targetContainer.SetActive(false);
        skillContainer.SetActive(false);
        comboContainer.SetActive(false);
        pickAction.SetActive(false);

        if (gameManager.enemies.Count <= 0) EndBattle();

        StartCoroutine(BattleSequence());
    }

    private IEnumerator BattleSequence()
    {
        while (true) {
            if (turnOrder.Count > 0)
            {
                CharacterInfo activeCharacter = turnOrder.Peek();

                if (activeCharacter == player)
                {
                    InitializePlayer();
                    while (awaitCommand)
                    {

                        yield return null;
                    }

                    int i = 0;
                    foreach (ComboAction a in comboActions)
                    {
                        actionText.text = $"{activeCharacter.characterName} used {a.Name} at {target.characterName}";
                        if (!activeCharacter.DoAction(a, target, i))
                        {
                            actionText.text = $"{activeCharacter.characterName} missed";
                            break;
                        }

                        while (player.isAttacking)
                        {
                            
                            yield return null;
                        }

                        UpdateHealth();

                        if (target.health <= 0)
                        {
                            killCount++;

                            int targetIndex = gameManager.enemies.IndexOf(target as EnemyInfo);
                            
                            targetContainer.transform.GetChild(targetIndex).GetComponent<Button>().interactable = false;
                            eHealthContainer.transform.GetChild(targetIndex).GetComponent<TextMeshProUGUI>().text = target.characterName + " is defeated";

                            var newTurnOrder = new Queue<CharacterInfo>(turnOrder.Where(x => x != target));
                            turnOrder = newTurnOrder;

                            Destroy(target.gameObject);

                            if (killCount >= gameManager.enemies.Count) EndBattle();

                            break;
                        }
                        i++;
                    }

                } else {
                    while (comboLength < activeCharacter.combo)
                    {
                        ComboAction comboAction = activeCharacter.PickEnemyCombo(comboLength);
                        comboActions.Add(comboAction);
                        comboLength += comboAction.Cost;
                    }
                    int i = 0;
                    foreach (ComboAction a in comboActions)
                    {
                        actionText.text = $"{activeCharacter.characterName} used {a.Name} at {player.characterName}";
                        if (!activeCharacter.DoAction(a, player, i))
                        {
                            actionText.text = $"{activeCharacter.characterName} missed";
                            break;
                        }

                        UpdateHealth();

                        if (player.health <= 0) EndBattle();

                        i++;

                        yield return new WaitForSeconds(.5f);
                    }
                    
                }

                NextTurn(activeCharacter);

                yield return new WaitForSeconds(.5f);

            }
        }
    }

    private void InitializePlayer()
    {
        UpdateSkills();
        actionText.text = "";
        pElement.text = player.characterName + "'s Active Element: " + player.element;
        awaitCommand = true;
        pickAction.SetActive(true);
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

    private void SelectEscape()
    {
        EndBattle();
    }

    private void BackFromSkill()
    {
        skillContainer.SetActive(false);
        pickAction.SetActive(true);
    }

    private void BackFromTarget()
    {
        player.activeSkill = null;
        player.element = SkillList.Element.None;
        pSkill.text = "";
        pElement.text = player.characterName + "'s Active Element: " + player.element;
        targetContainer.SetActive(false);
        pickAction.SetActive(true);
    }

    private void BackFromCombo()
    {
        comboContainer.SetActive(false);
        comboActions.Clear();
        comboLength = 0;
        UpdateCombo();
        targetContainer.SetActive(true);
    }

    private void PickTarget(CharacterInfo target)
    {
        this.target = target;
        targetContainer.SetActive(false);
        comboContainer.SetActive(true);
    }

    private void PickCombo(ComboAction action)
    {
        if (action.Cost <= player.combo - comboLength) 
        {
            comboActions.Add(action);
            comboLength += action.Cost;
        }

        if (comboLength < player.combo) UpdateCombo();

        else
        {
            pCombo.text = player.characterName + "'s Combo Length: " + (player.combo - comboLength);
            awaitCommand = false;
            comboContainer.SetActive(false);
        } 
    }

    private void PickSkill(SkillAction skill)
    {
        player.UseSkill(skill);

        skillContainer.SetActive(false);
        targetContainer.SetActive(true);
        pSkill.text = player.characterName + "'s Active Skill: " + player.activeSkill.Name;
        pElement.text = player.characterName + "'s Active Element: " + player.element;
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

        Button back = Instantiate(backButton, targetContainer.transform);
        back.onClick.AddListener(BackFromTarget);
    }

    private void SetCombos()
    {
        for (int i = 0; i < player.CountActions(); i++)
        {
            ComboAction currentCombo = player.GetCombo(i);
            Button selectCombo = Instantiate(comboButton, comboContainer.transform);
            selectCombo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCombo.Name;
            selectCombo.onClick.AddListener(() => PickCombo(currentCombo));
            comboButtons.Add(selectCombo);
        }

        Button back = Instantiate(backButton, comboContainer.transform);
        back.onClick.AddListener(BackFromCombo);
    }

    private void UpdateSkills()
    {
        for (int i = 0; i < skillContainer.transform.childCount; i++)
        {
            Destroy(skillContainer.transform.GetChild(i).gameObject);
        }

        pSkill.text = "";

        for (int i = 0; i < player.CountSkills(); i++)
        {
            SkillAction currentSkill = player.GetSkill(i);
            Button selectSkill = Instantiate(skillButton, skillContainer.transform);
            selectSkill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentSkill.Name;
            selectSkill.onClick.AddListener(() => PickSkill(currentSkill));
            skillButtons.Add(selectSkill);
        }
        Button back = Instantiate(backButton, skillContainer.transform);
        back.onClick.AddListener(BackFromSkill);
    }

    private void UpdateHealth()
    {
        pHealth.text = player.characterName + "'s Health: " + player.health;

        for (int i = 0; i < gameManager.enemies.Count; i++)
        {
            EnemyInfo enemy = gameManager.enemies[i];

            if (enemy.health > 0)
                eHealthContainer.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = enemy.characterName + "'s Health: " + enemy.health;
        }
    }

    private void UpdateCombo()
    {
        for (int i = 0; i < comboButtons.Count; i++) 
        {
            comboButtons[i].interactable = player.GetCombo(i).Cost <= player.combo - comboLength;
        }

        pCombo.text = player.characterName + "'s Combo Length: " + (player.combo - comboLength);
    }

    private void NextTurn(CharacterInfo activeCharacter)
    {
        turnOrder.Dequeue();
        turnOrder.Enqueue(activeCharacter);

        comboActions.Clear();
        comboLength = 0;

        UpdateCombo();

        if (gameManager.enemies.Count <= 0) EndBattle();
    }

    private void OnDisable() {
        Debug.Log("End Battle");

        StopAllCoroutines();

        gameManager.enemies.Clear();

        ClearUI();

        if (player != null)
        {
            player.gameObject.GetComponent<PlayerMovement>().enabled = true;
            player.element = SkillList.Element.None;
        }
    }

    private void ClearUI()
    {
        foreach (TextMeshProUGUI t in eHealthContainer.GetComponentsInChildren<TextMeshProUGUI>()){
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
    }

    public void EndBattle()
    {
        player.EndCombat();
        gameObject.SetActive(false);
    }
}