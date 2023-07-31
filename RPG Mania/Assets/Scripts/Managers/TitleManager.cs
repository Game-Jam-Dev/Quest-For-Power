using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    [SerializeField] private Button newButton, loadButton, settingsButton, creditsButton, quitButton;
    private int nextScene = 4;
    private int creditsScene = 5;

    private void Awake() {
        newButton.onClick.AddListener(StartGame);
        loadButton.onClick.AddListener(LoadGame);
        settingsButton.onClick.AddListener(Settings);
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(QuitGame);

        loadButton.interactable = SaveSystem.SaveFileExists();
        settingsButton.interactable = false;
    }

    private void StartGame(){
        SceneManager.LoadScene(nextScene);
    }

    private void LoadGame()
    {
        GameData gameData = SaveSystem.LoadGameData(); // Load saved game data
        if(gameData != null)
        {
            GameManager.instance.SetGameData(gameData);

            SceneManager.LoadScene(gameData.worldState.currentScene);
        }
    }

    private void Settings(){}

    private void Credits()
    {
        SceneManager.LoadScene(creditsScene);
    }

    private void QuitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}