using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public PlayerInfo player;

    public void SetPlayer(GameObject player)
    {
        this.player = player.GetComponent<PlayerInfo>();
        this.player.SetStats(1);
        this.player.ResetHealth();
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