using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour {
    [SerializeField] private GameObject pauseUI, battleUI;
    [SerializeField] private Button resumeButton, quitButton, battleButton;

    private InputActions actions;
    
    private int mainMenuSceneIndex = 0;

    private void Awake() {
        actions = new InputActions();

        actions.Gameplay.Enable();

        actions.Gameplay.Pause.performed += TogglePause;

        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
    }

    private void OnEnable() {
        Resume();
    }

    private void OnDisable() {
        actions.Gameplay.Pause.performed -= TogglePause;
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
        if (battleButton != null) battleButton.enabled = true;
    }

    private void Pause() {
        Time.timeScale = 0; 
        pauseUI.SetActive(true);
        if (battleButton != null) battleButton.enabled = false;
    }

    private void Quit() {
        Time.timeScale = 1;
        actions.Gameplay.Disable();
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}