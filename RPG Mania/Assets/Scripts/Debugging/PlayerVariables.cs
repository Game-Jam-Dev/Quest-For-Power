using UnityEngine;

public class PlayerVariables : MonoBehaviour {
    private PlayerInfo playerInfo;
    private void Start() {
        playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();;
    }

    private void OutputIntToString(string value, out int output)
    {
        if (int.TryParse(value, out int n))
            output = n;
        else 
            output = 0;
    }

    private void OutputFloatToString(string value, out float output)
    {
        if (float.TryParse(value, out float n))
            output = n;
        else
            output = 0;
    }

    public void ChangeHealth(string value)
    {
        OutputIntToString(value, out playerInfo.maxHealth);
        playerInfo.ResetHealth();
    }

    public void ChangeAttack(string value)
    {
        OutputIntToString(value, out playerInfo.attack);
    }

    public void ChangeDefense(string value)
    {
        OutputIntToString(value, out playerInfo.defense);
    }
    
    public void ChangeAccuracy(string value)
    {
        OutputFloatToString(value, out playerInfo.accuracy);
    }

    public void ChangeEvasion(string value)
    {
        OutputFloatToString(value, out playerInfo.evasion);
    }

    public void ChangeCombo(string value)
    {
        OutputIntToString(value, out playerInfo.combo);
    }

    public void ChangeLevel(string value)
    {
        int level;
        OutputIntToString(value, out level);

        if (level != 0) playerInfo.SetStats(level);
    }

    public void Reset()
    {
        playerInfo.SetStats(GameManager.instance.GetGameData().playerData.level);
    }
}