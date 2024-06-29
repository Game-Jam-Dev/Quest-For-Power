using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;
using TMPro;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour {
    public static PauseManager Instance;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button saveButton, quitButton;
    [SerializeField] private GameObject defaultContainer, itemsContainer, spellsContainer, statusContainer, equipmentContainer, 
        storyContainer, tutorialContainer, saveContainer, settingsContainer, enumerationBackground, descriptionContainer;

    [SerializeField]
    private TextMeshProUGUI itemButtonText, spellButtonText, statusButtonText, equipmentButtonText,
        storyButtonText, tutorialButtonText, saveButtonText, settingsButtonText;

    private List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>();

    public static event Action<bool> pauseEvent;

    private InputActions actions;
    
    private int mainMenuSceneIndex = 0;

    private bool itemScreen = false;
    private bool spellScreen = false;
    private bool statusScreen = false;
    private bool equipementScreen = false;
    private bool storyScreen = false;
    private bool tutorialScreen = false;
    private bool settingsScreen = false;

    private void Awake() {
        
        Instance = this;

        actions = new InputActions();

        actions.Gameplay.Enable();

        actions.Gameplay.Pause.performed += TogglePause;
    }

    private void Start()
    {
        buttonTexts.Add(itemButtonText);
        buttonTexts.Add(spellButtonText);
        buttonTexts.Add(statusButtonText);
        buttonTexts.Add(equipmentButtonText);
        buttonTexts.Add(storyButtonText);
        buttonTexts.Add(tutorialButtonText);
        buttonTexts.Add(saveButtonText);
        buttonTexts.Add(settingsButtonText);
        GrayOutAllButtons();
    }

    private void GrayOutAllButtons()
    {
        foreach (TextMeshProUGUI text in buttonTexts)
        {
            text.color = Color.gray;
        }
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
        pauseUI.SetActive(false);
        if (pauseEvent != null)
        {
            pauseEvent(false);
        }
        ClearItemInstances();
        ClearSpellInstances();
    }

    private void Pause() {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
        pauseEvent(true);
        CloseStatusUI();
        GrayOutAllButtons();
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

    public void EnablePausing()
    {
        actions.Gameplay.Pause.performed += TogglePause;
    }

    private void TurnOffScreenBools(string screen)
    {
        if (screen != "item")
        {
            itemScreen = false;
        }
        if (screen != "spell")
        {
            spellScreen = false;
        }
        if (screen != "status")
        {
            statusScreen = false;
        }
        if (screen != "equipment")
        {
            equipementScreen = false;
        }
        if (screen != "story")
        {
            storyScreen = false;
        }
        if (screen != "tutorial")
        {
            tutorialScreen = false;
        }
        if (screen != "settings")
        {
            settingsScreen = false;
        }            
    }

    public void ToggleItems()
    {
        GrayOutAllButtons();
        TurnOffScreenBools("item");
        if (!itemScreen)
        {
            ClearSpellInstances();
            ClearItemInstances();
            CloseStandardUI();
            CloseSpellsUI();
            CloseStatusUI();
            OpenItemUI();
            pauseUI.GetComponent<PauseMenu>().DisplayItems();
            itemButtonText.color = Color.white;
            OpenEnumerationUI();
        }
        else
        {
            CloseItemUI();
            ClearItemInstances();
            OpenStandardUI();
            CloseEnumerationUI();
        }
        itemScreen = !itemScreen;
    }

    public void ClearItemInstances()
    {
        pauseUI.GetComponent<PauseMenu>().ClearItems();
    }

    public void ToggleSpells()
    {
        GrayOutAllButtons();
        TurnOffScreenBools("spell");
        if (!spellScreen)
        {
            ClearItemInstances();
            ClearSpellInstances();
            CloseItemUI();
            CloseStandardUI();
            CloseStatusUI();
            OpenSpellsUI();
            pauseUI.GetComponent<PauseMenu>().DisplaySpells();
            spellButtonText.color = Color.white;
        }
        else
        {
            CloseSpellsUI();
            ClearSpellInstances();
            OpenStandardUI();
            CloseEnumerationUI();
        }
        spellScreen=!spellScreen;
    }

    public void ClearSpellInstances()
    {
        pauseUI.GetComponent<PauseMenu>().ClearSpells();
    }

    public void ToggleStatus()
    {
        GrayOutAllButtons();
        TurnOffScreenBools("status");
        if (!statusScreen)
        {
            CloseStatusUI();
            ClearItemInstances();
            ClearSpellInstances();
            CloseItemUI();
            CloseStandardUI();
            CloseSpellsUI();
            OpenStatusUI();
            pauseUI.GetComponent<PauseMenu>().DisplayStatus();
            statusButtonText.color = Color.white;
        }
        else
        {
            CloseStatusUI();
            OpenStandardUI();
        }
        statusScreen = !statusScreen;
    }

    public void OpenStatusUI()
    {
        statusContainer.SetActive(true);
    }

    public void CloseStatusUI()
    {
        statusContainer.SetActive(false);
    }

    public void OpenEnumerationUI() 
    {
        enumerationBackground.SetActive(true); 
        descriptionContainer.SetActive(true);
    }

    public void CloseEnumerationUI()
    {
        enumerationBackground.SetActive(false);
        descriptionContainer.SetActive(false);
    }

    public void OpenItemUI()
    {
        OpenEnumerationUI();
        itemsContainer.SetActive(true);
    }
    public void CloseItemUI()
    {
        if (itemsContainer != null)
        {
            itemsContainer.SetActive(false);
        }
        CloseEnumerationUI();
    }

    public void OpenStandardUI()
    {
        defaultContainer.SetActive(true);
    }
    
    public void CloseStandardUI()
    {
        defaultContainer.SetActive(false);
    }

    public void CloseUI()
    {
        OpenStandardUI();
        CloseItemUI();
        CloseSpellsUI();
        itemScreen = false;
        spellScreen = false;
    }

    public void OpenSpellsUI()
    {
        OpenEnumerationUI();
        spellsContainer.SetActive(true);
    }

    public void CloseSpellsUI()
    {
        CloseEnumerationUI();
        spellsContainer.SetActive(false);
    }
}