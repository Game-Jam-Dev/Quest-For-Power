using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour {
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button resumeButton, saveButton, quitButton;
    [SerializeField] private GameObject portrait, characterName, itemList, itemQuantities, itemDescription, 
        itemBackground, itemContainer, spellsBackground, miniPortrait, spellColumnName, spellColumnQty, miniDescription;

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
        CloseUI();
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
        ClearItemInstances();
        OpenItemUI();
        CloseStandardUI();
        CloseSpellsUI();
        pauseUI.GetComponent<PauseMenu>().DisplayItems();
    }

    public void ClearItemInstances()
    {
        pauseUI.GetComponent<PauseMenu>().ClearItems();
    }

    public void OpenSpells()
    {
        ClearItemInstances();
        CloseItemUI();
        CloseStandardUI();
        OpenSpellsUI();
        pauseUI.GetComponent<PauseMenu>().DisplaySpells();
    }

    public void OpenItemUI()
    {
        itemList.SetActive(true);
        itemDescription.SetActive(true);
        itemQuantities.SetActive(true);
        itemBackground.SetActive(true);
        itemContainer.SetActive(true);
    }
    public void CloseItemUI()
    {
        itemList.SetActive(false);
        itemDescription.SetActive(false);
        itemQuantities.SetActive(false);
        itemBackground.SetActive(false);
        itemContainer.SetActive(false);
    }

    public void OpenStandardUI()
    {
        portrait.SetActive(true);
        characterName.SetActive(true);
    }
    
    public void CloseStandardUI()
    {
        portrait.SetActive(false);
        characterName.SetActive(false);
    }

    public void CloseUI()
    {
        OpenStandardUI();
        CloseItemUI();
        CloseSpellsUI();
    }

    public void OpenSpellsUI()
    {
        spellsBackground.SetActive(true);
        miniPortrait.SetActive(true);
        spellColumnName.SetActive(true);
        spellColumnQty.SetActive(true);
        miniDescription.SetActive(true);
    }

    public void CloseSpellsUI()
    {
        spellsBackground.SetActive(false);
        miniPortrait.SetActive(false);
        spellColumnName.SetActive(false);
        spellColumnQty.SetActive(false);
        miniDescription.SetActive(false);

    }
}