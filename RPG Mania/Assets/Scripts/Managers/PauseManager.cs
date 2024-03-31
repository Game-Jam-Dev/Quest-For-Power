using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour {
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button resumeButton, saveButton, quitButton;
    [SerializeField] private GameObject portrait, characterNameTag, itemList, itemQuantities, itemDescription, 
        itemBackground, itemContainer, characterDetailsBackground, miniPortraitBackground, spellsContainer, spellQuantity, spellDescription,
        StatusDetails;

    public static event Action<bool> pauseEvent;

    private InputActions actions;
    
    private int mainMenuSceneIndex = 0;

    public bool pauseAllowed = true;

    private void Awake() 
    {
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
        CloseStatusUI();
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
        ClearSpellInstances();
        ClearItemInstances();        
        CloseStandardUI();
        CloseSpellsUI();
        CloseStatusUI();
        OpenItemUI();
        pauseUI.GetComponent<PauseMenu>().DisplayItems();
    }

    public void ClearItemInstances()
    {
        pauseUI.GetComponent<PauseMenu>().ClearItems();
    }

    public void OpenSpells()
    {
        ClearItemInstances();
        ClearSpellInstances();
        CloseItemUI();
        CloseStandardUI();
        CloseStatusUI();
        OpenSpellsUI();
        pauseUI.GetComponent<PauseMenu>().DisplaySpells();
    }

    public void ClearSpellInstances()
    {
        pauseUI.GetComponent<PauseMenu>().ClearSpells();
    }

    public void OpenStatus()
    {
        CloseStatusUI();
        ClearItemInstances();
        ClearSpellInstances();
        CloseItemUI();
        CloseStandardUI();
        CloseSpellsUI();
        OpenStatusUI();
        pauseUI.GetComponent<PauseMenu>().DisplayStatus();
    }

    public void OpenStatusUI()
    {
        characterDetailsBackground.SetActive(true);
        StatusDetails.SetActive(true);
    }

    public void CloseStatusUI()
    {
        characterDetailsBackground.SetActive(false);
        StatusDetails.SetActive(false);
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
        characterNameTag.SetActive(true);
    }
    
    public void CloseStandardUI()
    {
        portrait.SetActive(false);
        characterNameTag.SetActive(false);
    }

    public void CloseUI()
    {
        OpenStandardUI();
        CloseItemUI();
        CloseSpellsUI();
    }

    public void OpenSpellsUI()
    {
        characterDetailsBackground.SetActive(true);
        miniPortraitBackground.SetActive(true);
        spellsContainer.SetActive(true);
        spellQuantity.SetActive(true);
        spellDescription.SetActive(true);
    }

    public void CloseSpellsUI()
    {
        characterDetailsBackground.SetActive(false);
        miniPortraitBackground.SetActive(false);
        spellsContainer.SetActive(false);
        spellQuantity.SetActive(false);
        spellDescription.SetActive(false);

    }
}