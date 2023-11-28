using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class WorldManager : MonoBehaviour {
    [SerializeField] protected GameObject battleUI;
    [SerializeField] protected DialogManager dialogManager;
    protected GameManager gameManager;
    protected GameObject player;
    protected PlayerBattle playerBattle;
    protected PlayerMovement playerMovement;
    protected List<EnemyBattle> enemies = new();

    protected virtual void Start() {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        gameManager.SetCurrentScene(currentScene);

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerBattle = player.GetComponent<PlayerBattle>();
        playerMovement = player.GetComponent<PlayerMovement>();

        SpawnEnemies();
    }

    public virtual List<EnemyBattle> GetBattleEnemies() 
    {
        return enemies; 
    }

    public virtual void PrepareCharactersForCombat(IEnumerable<CharacterBattle> characters)
    {
        foreach (CharacterBattle c in characters)
            c.PrepareCombat();
    }

    public virtual void WinBattle(){}

    public virtual void LoseBattle()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public virtual void EscapeBattle(){}

    protected virtual void SpawnEnemies()
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(e.GetComponent<EnemyBattle>());
        }
    }

    protected virtual IEnumerator DoDialog(DialogObject dialogObject)
    {
        playerMovement.enabled = false;

        dialogManager.enabled = true;
        
        // Start the next dialog.
        dialogManager.DisplayDialog(dialogObject);

        // Wait for this dialog to finish before proceeding to the next one.
        yield return new WaitUntil(() => !dialogManager.ShowingDialog());
        
        dialogManager.enabled = false;

        playerMovement.enabled = true;
    }

    public PlayerBattle GetPlayer() { return playerBattle; }
}