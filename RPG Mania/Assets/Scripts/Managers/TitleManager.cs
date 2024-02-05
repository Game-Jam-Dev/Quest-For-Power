using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    [SerializeField] private Button loadButton;
    [SerializeField] private GameObject settingsMenu;
    private GameData gameData;
    private int introScene = 1;
    private int pickScene = 7;
    private int creditsScene = 6;

    private void Awake() 
    {
        gameData = SaveSystem.LoadGameData();

        if (gameData == null)
        {
            loadButton.interactable = false;
            gameData = new GameData();
        }

        // settingsMenu.GetComponent<SettingsMenuManager>().Initialize(gameData.settingsData);
    }

    public void StartGame()
    {
        EnterGame(gameData.NewGame(), introScene);
    }

    public void LoadGame()
    {
        if(gameData != null)
        {
            EnterGame(gameData, gameData.worldState.currentScene);
        }
    }

    private void EnterGame(GameData gameData, int scene)
    {
        GameManager.instance.SetGameData(gameData);

        SceneManager.LoadScene(scene);
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Credits()
    {
        SceneManager.LoadScene(creditsScene);
    }

    public void QuitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void UpdateSettings(SettingsData settingsData)
    {
        gameData.settingsData = settingsData;
        SaveSystem.SaveGameData(gameData);
    }
}