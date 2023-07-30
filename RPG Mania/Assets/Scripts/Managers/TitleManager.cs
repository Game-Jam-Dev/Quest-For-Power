using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    [SerializeField] private Button newButton, loadButton, settingsButton, creditsButton, quitButton;
    private int nextScene = 4;

    private void Awake() {
        newButton.onClick.AddListener(StartGame);
        loadButton.onClick.AddListener(LoadGame);
        settingsButton.onClick.AddListener(Settings);
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(QuitGame);

        loadButton.interactable = false;
        settingsButton.interactable = false;
        creditsButton.interactable = false;
    }

    private void StartGame(){
        SceneManager.LoadScene(nextScene);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(nextScene);
    }

    private void Settings(){}

    private void Credits(){}

    private void QuitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}