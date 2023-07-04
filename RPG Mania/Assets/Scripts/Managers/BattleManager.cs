using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI pHealth, eHealth;
    [SerializeField] private GameObject eHealthContainer, actionContainer, targetContainer;
    [SerializeField] private Button actionButton, targetButton;
    private GameManager gameManager;
    private int worldScene = 1;
    public int killCount = 0;

    private Queue<CharacterInfo> turnOrder = new Queue<CharacterInfo>();
    private bool awaitCommand = false;

    private CharacterAction action;
    private CharacterInfo target;

    private Coroutine battle;

    private void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    private void Start() {

        var characters = new List<CharacterInfo> { gameManager.player }.Concat(gameManager.enemies);
        var sortedCharacters = characters.OrderByDescending(c => c.speed);
        turnOrder = new Queue<CharacterInfo>(sortedCharacters);

        for (int i = 0; i < gameManager.enemies.Count; i++)
        {
            EnemyInfo enemy = gameManager.enemies[i];

            TextMeshProUGUI enemyHealthText = Instantiate(eHealth, eHealthContainer.transform);

            enemyHealthText.rectTransform.anchoredPosition = new Vector3(0, -i * 100);

            Button selectEnemy = Instantiate(targetButton, targetContainer.transform);

            selectEnemy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.characterName;

            selectEnemy.onClick.AddListener(() => PickTarget(enemy));
        }

        for (int i = 0; i < gameManager.player.actions.Count; i++)
        {
            CharacterAction currentAction = gameManager.player.GetAction(i);

            Button selectAction = Instantiate(actionButton, actionContainer.transform);

            selectAction.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentAction.Name;

            selectAction.onClick.AddListener(() => PickAction(currentAction));
        }

        targetContainer.SetActive(false);
        actionContainer.SetActive(false);

        UpdateHealth();

        battle = StartCoroutine(BattleSequence());
    }

    private void PickTarget(CharacterInfo target)
    {
        this.target = target;
        awaitCommand = false;
    }

    private void PickAction(CharacterAction action)
    {
        this.action = action;
        awaitCommand = false;
    }

    private void UpdateHealth()
    {
        pHealth.text = "Player Health: " + gameManager.player.health;

        for (int i = 0; i < gameManager.enemies.Count; i++)
        {
            eHealthContainer.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "Enemy Health: " + gameManager.enemies[i].health;
        }
    }

    private IEnumerator BattleSequence()
    {
        while (true){
            if (turnOrder.Count > 0)
            {
                var activeCharacter = turnOrder.Peek();

                if (activeCharacter == gameManager.player){
                    awaitCommand = true;
                    actionContainer.SetActive(true);
                    while (awaitCommand)
                    {

                        
                        yield return null;
                    }
                    awaitCommand = true;
                    actionContainer.SetActive(false);
                    targetContainer.SetActive(true);
                    while (awaitCommand)
                    {


                        yield return null;
                    }

                    targetContainer.SetActive(false);

                    Debug.Log($"{activeCharacter.characterName} used {action.Name} at {target.characterName}"); // Display the action's name
                    action.Action(target);

                    if (target.health <= 0)
                    {
                        killCount++;

                        if (killCount >= gameManager.enemies.Count) EndBattle();

                        int targetIndex = gameManager.enemies.IndexOf(target as EnemyInfo);
                        
                        targetContainer.transform.GetChild(targetIndex).GetComponent<Button>().enabled = false;
                        
                    }

                } else {
                    action = activeCharacter.actions[0];

                    Debug.Log($"{activeCharacter.characterName} used {action.Name} at {gameManager.player.characterName}"); // Display the action's name
                    action.Action(gameManager.player);

                    if (gameManager.player.health <= 0) EndBattle();
                }

                

                UpdateHealth();

                turnOrder.Dequeue();
                turnOrder.Enqueue(activeCharacter);

            }
            
            
        }
        
    }

    public void EndBattle()
    {
        Debug.Log("End Battle");

        StopCoroutine(battle);

        gameManager.enemies.Clear();

        SceneManager.LoadScene(worldScene);
    }
}