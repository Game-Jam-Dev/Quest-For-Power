using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour {
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button resumeButton, saveButton, quitButton;

    private InputActions actions;
    
    private int mainMenuSceneIndex = 0;

    private void Awake() {
        actions = new InputActions();

        actions.Gameplay.Enable();

        actions.Gameplay.Pause.performed += TogglePause;

        resumeButton.onClick.AddListener(Resume);
        saveButton.onClick.AddListener(SaveGame);
        quitButton.onClick.AddListener(Quit);
    }

    private void OnEnable() {
        Resume();
    }

    private void OnDisable() {
        actions.Gameplay.Pause.performed -= TogglePause;

        actions.Gameplay.Disable();
    }

    private void TogglePause(InputAction.CallbackContext context) {
        if (pauseUI.activeSelf)
            Resume();
        else
            Pause();
    }

    private void Resume() {
        Time.timeScale = 1; 
        pauseUI.SetActive(false);
    }

    private void Pause() {
        Time.timeScale = 0; 
        pauseUI.SetActive(true);
    }

    private void SaveGame()
    {
        SaveSystem.SaveGameData(GameManager.instance.GetGameData());
        
        GameManager.instance.GetPlayerSkillUses();
    }

    private void Quit() {
        Time.timeScale = 1;
        actions.Gameplay.Disable();
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void DisablePausing()
    {
        actions.Gameplay.Pause.performed -= TogglePause;
    }
}