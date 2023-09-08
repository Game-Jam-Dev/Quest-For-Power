using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    [SerializeField] private Button newButton, loadButton, creditsButton, quitButton;
    private int throneScene = 1;
    private int pickScene = 4;
    private int creditsScene = 5;

    private void Awake() {
        newButton.onClick.AddListener(StartGame);
        loadButton.onClick.AddListener(LoadGame);
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(QuitGame);

        loadButton.interactable = SaveSystem.SaveFileExists();
    }

    public void StartGame(){
        EnterGame(new GameData(), pickScene);
    }

    public void LoadGame()
    {
        GameData gameData = SaveSystem.LoadGameData(); // Load saved game data
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