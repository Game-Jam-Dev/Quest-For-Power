using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class BattleManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI pHealth, pSkill, pElement, pCombo, actionText, eHealth;
    [SerializeField] private GameObject eHealthContainer, comboContainer, skillContainer, targetContainer, pickAction;
    [SerializeField] private Button pickAttackButton, pickAbsorbButton, pickSkillButton, pickEscapeButton, comboButtonPrefab, skillButtonPrefab, targetButtonPrefab, backButtonPrefab;
    [SerializeField] private WorldManager worldManager;
    private List<Button> targetButtons = new();
    private List<Button> comboButtons = new();
    private List<Button> skillButtons = new();
    private GameManager gameManager;
    public int killCount = 0;
    private int xpGain = 0;
    private float dialogueDisplayTime = 1.5f;

    private Queue<CharacterInfo> turnOrder = new Queue<CharacterInfo>();
    public List<EnemyInfo> enemies = new List<EnemyInfo>();

    private bool awaitCommand = false;
    private int comboLength = 0;
    private bool canCombo = true;
    private int absorbCounter = 0;
    private int activeSkillCounter = 0;

    private List<ComboAction> comboActions = new();
    private CharacterInfo target;
    private PlayerInfo player;

    private Coroutine battleLoop;

    private void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameObject.SetActive(false);
    }
    
    private void OnEnable() {
        player = gameManager.player;
        player.gameObject.GetComponent<PlayerMovement>().enabled = false;

        enemies = worldManager.GetEnemies();
        enemies.Reverse();
        var characters = new List<CharacterInfo> { player }.Concat(enemies);
        turnOrder = new Queue<CharacterInfo>(characters);
        
        pickAbsorbButton.interactable = true;

        SetEnemies();
        SetCombos();

        UpdateCombo();
        UpdateSkills();
        UpdateHealth();

        targetContainer.SetActive(false);
        skillContainer.SetActive(false);
        comboContainer.SetActive(false);
        pickAction.SetActive(false);

        if (enemies.Count <= 0) EndBattle();

        battleLoop = StartCoroutine(BattleSequence());
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

                    if (!canCombo)
                    {
                        player.AbsorbSkill(target.element);
                        absorbCounter += 1;
                        actionText.text = $"{activeCharacter.characterName} absorbed the {target.element} element from {target.characterName}";

                        if (absorbCounter >= 3) pickAbsorbButton.interactable = false;

                        UpdateHealth();
                    } 
                    else
                    {
                        int i = 0;
                        foreach (ComboAction a in comboActions)
                        {
                            actionText.text = $"{activeCharacter.characterName} used {a.Name} at {target.characterName}";
                            bool hit = activeCharacter.DoAction(a, target, i);
                                
                            while (activeCharacter.GetIsAttacking())
                            {
                                
                                yield return null;
                            }

                            target.Recover();

                            if (!hit)
                            {
                                actionText.text = $"{activeCharacter.characterName} missed";
                                break;
                            }

                            UpdateHealth();

                            if (target.health <= 0)
                            {
                                killCount++;

                                xpGain += (target as EnemyInfo).XPFromKill(player.level);

                                int targetIndex = enemies.IndexOf(target as EnemyInfo);
                                
                                targetContainer.transform.GetChild(targetIndex).GetComponent<Button>().interactable = false;
                                eHealthContainer.transform.GetChild(targetIndex).GetComponent<TextMeshProUGUI>().text = target.characterName + " is defeated";

                                var newTurnOrder = new Queue<CharacterInfo>(turnOrder.Where(x => x != target));
                                turnOrder = newTurnOrder;

                                target.Defeated();

                                if (killCount >= enemies.Count) 
                                    StartCoroutine(WinBattle());

                                break;
                            }
                            i++;
                        }
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
                        bool hit = activeCharacter.DoAction(a, player, i);
                            
                        while (activeCharacter.GetIsAttacking())
                        {
                            
                            yield return null;
                        }

                        if (!hit)
                        {
                            actionText.text = $"{activeCharacter.characterName} missed";
                            break;
                        }

                        UpdateHealth();

                        if (player.health <= 0)
                            StartCoroutine(LoseBattle());
                        i++;
                    }
                }

                yield return new WaitForSeconds(.5f);

                NextTurn(activeCharacter);
            }

            yield return null;
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

    public void SelectAttack()
    {
        canCombo = true;
        pickAction.SetActive(false);
        targetContainer.SetActive(true);
    }

    public void SelectAbsorb()
    {
        canCombo = false;
        pickAction.SetActive(false);
        targetContainer.SetActive(true);
    }

    public void SelectSkill()
    {
        canCombo = true;
        pickAction.SetActive(false);
        skillContainer.SetActive(true);
    }

    public void SelectEscape()
    {
        StopCoroutine(battleLoop);
        worldManager.EscapeBattle();
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

        if (canCombo) comboContainer.SetActive(true);

        else awaitCommand = false;
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
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyInfo enemy = enemies[i];
            TextMeshProUGUI enemyHealthText = Instantiate(eHealth, eHealthContainer.transform);
            enemyHealthText.rectTransform.anchoredPosition = new Vector3(0, -i * 100);

            Button selectEnemy = Instantiate(targetButtonPrefab, targetContainer.transform);
            string elementText = "";
            if (enemy.element != SkillList.Element.None) elementText = enemy.element + " ";

            selectEnemy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = elementText + enemy.characterName;
            selectEnemy.onClick.AddListener(() => PickTarget(enemy));
            targetButtons.Add(selectEnemy);
        }

        Button back = Instantiate(backButtonPrefab, targetContainer.transform);
        back.onClick.AddListener(BackFromTarget);
    }

    private void SetCombos()
    {
        for (int i = 0; i < player.CountActions(); i++)
        {
            ComboAction currentCombo = player.GetCombo(i);
            Button selectCombo = Instantiate(comboButtonPrefab, comboContainer.transform);
            selectCombo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCombo.Name;
            selectCombo.onClick.AddListener(() => PickCombo(currentCombo));
            comboButtons.Add(selectCombo);
        }

        Button back = Instantiate(backButtonPrefab, comboContainer.transform);
        back.onClick.AddListener(BackFromCombo);
    }

    private void UpdateSkills()
    {
        for (int i = 0; i < skillContainer.transform.childCount; i++)
        {
            Destroy(skillContainer.transform.GetChild(i).gameObject);
        }

        if (player.activeSkill != null)
        {
            if (activeSkillCounter == 0)
                player.LoseSkill();

            if (activeSkillCounter < 3)
                activeSkillCounter++;
            else 
            {
                player.DeactivateSkill();
                pSkill.text = "";
                activeSkillCounter = 0;
            }
        } else {
            pSkill.text = "";
        }
        
        for (int i = 0; i < player.CountSkills(); i++)
        {
            SkillAction currentSkill = player.GetSkill(i);
            if (player.CanUseSkill(currentSkill))
            { 
                Button selectSkill = Instantiate(skillButtonPrefab, skillContainer.transform);

                int skillAmount = player.GetSkillAmount(currentSkill);
                string skillDisplay = currentSkill.Name + " (" + skillAmount + " ";
                if (skillAmount == 1)
                    skillDisplay += "Use)";
                else 
                    skillDisplay += "Uses)";

                selectSkill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = skillDisplay;
                selectSkill.onClick.AddListener(() => PickSkill(currentSkill));
                skillButtons.Add(selectSkill);
            }
        }

        pickSkillButton.interactable = skillContainer.transform.childCount > 0;

        if (pickSkillButton.interactable)
        {
            Button back = Instantiate(backButtonPrefab, skillContainer.transform);
            back.onClick.AddListener(BackFromSkill); 
        }
    }

    private void UpdateHealth()
    {
        pHealth.text = player.characterName + "'s Health: " + player.health + " Level " + player.level;

        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyInfo enemy = enemies[i];

            if (enemy.health > 0)
            {
                string elementText = "";
                if (enemy.element != SkillList.Element.None) elementText = enemy.element + " ";
                eHealthContainer.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = elementText + enemy.characterName + "'s Health: " + enemy.health;
            }
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

        comboButtons.Clear();
    }

    private void EndBattle()
    {
        player.EndCombat();

        StopAllCoroutines();

        enemies.Clear();

        xpGain = 0;
        absorbCounter = 0;
        killCount = 0;
        comboLength = 0;
        comboActions.Clear();

        ClearUI();

        if (player != null)
        {
            player.gameObject.GetComponent<PlayerMovement>().enabled = true;
            player.element = SkillList.Element.None;
        }

        gameObject.SetActive(false);
    }

    private IEnumerator WinBattle()
    {
        StopCoroutine(battleLoop);

        actionText.text = "You won!";

        int currentLevel = player.level;

        player.WinBattle(xpGain, killCount);

        yield return new WaitForSeconds(dialogueDisplayTime);

        if (player.level > currentLevel)
        {
            actionText.text = "Level up! You are now level " + player.level;
            yield return new WaitForSeconds(dialogueDisplayTime);
        }

        foreach (EnemyInfo e in enemies)
        {
            e.Kill();
        }

        worldManager.WinBattle();

        EndBattle();
    }

    private IEnumerator LoseBattle()
    {
        StopCoroutine(battleLoop);

        actionText.text = "You were defeated!";

        yield return new WaitForSeconds(dialogueDisplayTime);

        worldManager.LoseBattle();
    }
}