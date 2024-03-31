using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour {
    [SerializeField] private List<Sprite> images;
    [SerializeField] private List<NextElement> imageLeadsTo;
    [SerializeField] private Image imageBackground;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private List<DialogObject> dialogObjects;

    private int currentImageIndex = -1;
    private int currentDialogIndex = 0;
    [SerializeField] private string nextScene;
    [SerializeField] private Button skipButton;
    private bool canSkip = true;

    public float autoSkipTime = 3f;

    private enum NextElement
    {
        Image,
        Dialog,
    }

    private void Start() 
    {
        dialogManager.enabled = false;

        NextImage();
    }

    private void OnEnable() {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.DisablePausing();
        }
    }

    private void OnDisable() {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.EnablePausing();
        }
    }

    private IEnumerator DoDialog(DialogObject dialogObject)
    {
        dialogManager.enabled = true;
        
        // Start the next dialog.
        dialogManager.DisplayDialog(dialogObject);

        yield return new WaitUntil(() => dialogManager.fullLine);

        float c = 0;

        while (dialogManager.ShowingDialog() || autoSkipTime > c)
        {
            c += Time.deltaTime;

            yield return null;
        }
        
        dialogManager.enabled = false;

        Utility.SetActiveButton(skipButton);

        currentDialogIndex++;

        NextImage();
    }

    private IEnumerator WaitAtImage()
    {
        float c = 0;

        while (Input.GetMouseButtonDown(0) && canSkip || autoSkipTime > c)
        {
            c += Time.deltaTime;

            yield return null;
        }

        canSkip = false;

        NextImage();
    }

    private void NextImage()
    {
        currentImageIndex++;

        if (currentImageIndex >= imageLeadsTo.Count || currentImageIndex >= images.Count)
        {
            EndCutscene();
            return;
        }
            

        imageBackground.sprite = images[currentImageIndex];

        if (imageLeadsTo[currentImageIndex] == NextElement.Dialog && currentDialogIndex < dialogObjects.Count)
            StartCoroutine(DoDialog(dialogObjects[currentDialogIndex]));
        else 
            StartCoroutine(WaitAtImage());
    }

    private void Update()
    {
        // Detects mouse button up and sets the flag to true
        if (Input.GetMouseButtonUp(0))
        {
            canSkip = true;
        }
    }

    public void EndCutscene()
    {
        SceneManager.LoadScene(nextScene);
    }
}