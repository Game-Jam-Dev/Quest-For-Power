using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour {
    public static PauseManager Instance;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button resumeButton, saveButton, quitButton;
    [SerializeField] private GameObject itemsContainer, spellsContainer, statusContainer,
        defaultContainer;


    public static event Action<bool> pauseEvent;

    private InputActions actions;
    
    private int mainMenuSceneIndex = 0;
    private bool randomAnimations =  false;
    [SerializeField] private PlayerCombatAnimation pauseAnimatorClass;
    [SerializeField] private Animator pauseAnim;

    private void Awake() {
        
        Instance = this;

        actions = new InputActions();

        actions.Gameplay.Enable();

        actions.Gameplay.Pause.performed += TogglePause;
    }

    private void Update()
    {
        if (randomAnimations && pauseAnimatorClass.CheckIfAnimationIsDone(pauseAnim))
        {
            pauseAnimatorClass.RandomAnimationChange();
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
        randomAnimations = false;
        CloseUI();
        pauseUI.SetActive(false);
        if (pauseEvent != null)
        {
            pauseEvent(false);
        }        

    }

    private void Pause() {
        Time.timeScale = 0; 
        pauseUI.SetActive(true);
        pauseEvent(true);
        CloseStatusUI();
        Utility.SetActiveButton(resumeButton);
        randomAnimations = true;
        pauseAnimatorClass.ChangeAnimationState("Idle");
    }

    public void SaveGame()
    {
        SaveSystem.SaveGameData(GameManager.instance.GetGameData());
    }

    public void Quit() {
        Time.timeScale = 1;
        actions.Gameplay.Disable();
        SceneManager.LoadScene(mainMenuSceneIndex);
        randomAnimations = false;
    }

    public void DisablePausing()
    {
        actions.Gameplay.Pause.performed -= TogglePause;
    }

    public void EnablePausing()
    {
        actions.Gameplay.Pause.performed += TogglePause;
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
        randomAnimations = false;
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
        randomAnimations = false;
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
        randomAnimations = false;
    }

    public void OpenStatusUI()
    {
        //characterDetailsBackground.SetActive(true);
        //StatusDetails.SetActive(true);
    }

    public void CloseStatusUI()
    {
        //characterDetailsBackground.SetActive(false);
        //StatusDetails.SetActive(false);
    }

    public void OpenItemUI()
    {
        itemsContainer.SetActive(true);
    }
    public void CloseItemUI()
    {
        if (itemsContainer != null)
        {
            itemsContainer.SetActive(false);
        }            
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
    }

    public void OpenSpellsUI()
    {
        //characterDetailsBackground.SetActive(true);
        //miniPortraitBackground.SetActive(true);
        //spellsContainer.SetActive(true);
        //spellQuantity.SetActive(true);
        //spellDescription.SetActive(true);
    }

    public void CloseSpellsUI()
    {
        //if (characterDetailsBackground != null & miniPortraitBackground != null & spellsContainer != null &
        //    spellQuantity != null & spellDescription != null)
        //{
        //    characterDetailsBackground.SetActive(false);
        //    miniPortraitBackground.SetActive(false);
        //    spellsContainer.SetActive(false);
        //    spellQuantity.SetActive(false);
        //    spellDescription.SetActive(false);
        //}
    }
}