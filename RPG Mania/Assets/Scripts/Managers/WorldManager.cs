using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class WorldManager : MonoBehaviour {
    [SerializeField] protected GameObject battleUI;
    [SerializeField] protected DialogManager dialogManager;
    protected GameManager gameManager;
    protected GameObject player;
    protected PlayerInfo playerInfo;
    protected PlayerMovement playerMovement;
    protected List<EnemyInfo> enemies = new();

    protected virtual void Start() {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        gameManager.SetCurrentScene(currentScene);

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerInfo = player.GetComponent<PlayerInfo>();
        playerMovement = player.GetComponent<PlayerMovement>();

        SpawnEnemies();
    }

    public virtual List<EnemyInfo> GetEnemies() { return enemies; }

    public virtual void WinBattle(){}

    protected virtual void SpawnEnemies()
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(e.GetComponent<EnemyInfo>());
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
}