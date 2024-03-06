using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour {
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button resumeButton, saveButton, quitButton;
    [SerializeField] private GameObject portrait, characterName, itemList, itemQuantities;

    public static event Action<bool> pauseEvent;

    private InputActions actions;
    
    private int mainMenuSceneIndex = 0;

    private void Awake() {
        actions = new InputActions();

        actions.Gameplay.Enable();

        actions.Gameplay.Pause.performed += TogglePause;
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

    public void Resume() {
        Time.timeScale = 1;
        portrait.SetActive(true);
        characterName.SetActive(true);
        itemList.SetActive(false);
        CloseItems();
        itemQuantities.SetActive(false);
        pauseUI.SetActive(false);
        pauseEvent(false);

    }

    private void Pause() {
        Time.timeScale = 0; 
        pauseUI.SetActive(true);
        pauseEvent(true);
        Utility.SetActiveButton(resumeButton);
    }

    public void SaveGame()
    {
        SaveSystem.SaveGameData(GameManager.instance.GetGameData());
    }

    public void Quit() {
        Time.timeScale = 1;
        actions.Gameplay.Disable();
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void DisablePausing()
    {
        actions.Gameplay.Pause.performed -= TogglePause;
    }

    public void OpenItems()
    {
        CloseItems();
        portrait.SetActive(false);
        characterName.SetActive(false);
        itemList.SetActive(true);
        itemQuantities.SetActive(true);
        pauseUI.GetComponent<PauseMenu>().DisplayItems();
    }

    public void CloseItems()
    {
        pauseUI.GetComponent<PauseMenu>().ClearItems();
    }
}