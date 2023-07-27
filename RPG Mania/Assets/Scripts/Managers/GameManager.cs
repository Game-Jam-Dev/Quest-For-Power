using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public PlayerInfo player;
    public List<EnemyInfo> enemies = new List<EnemyInfo>();

    public void SetPlayer(GameObject player)
    {
        this.player = player.GetComponent<PlayerInfo>();
        this.player.SetStats(50);
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy.GetComponent<EnemyInfo>());
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy.GetComponent<EnemyInfo>());
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}