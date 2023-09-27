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
    private bool canSkip = true;

    private enum NextElement
    {
        Image,
        Dialog,
    }

    private void Awake() {
        dialogManager.enabled = false;

        NextImage();
    }

    private IEnumerator DoDialog(DialogObject dialogObject)
    {
        dialogManager.enabled = true;
        
        // Start the next dialog.
        dialogManager.DisplayDialog(dialogObject);

        // Wait for this dialog to finish before proceeding to the next one.
        yield return new WaitUntil(() => !dialogManager.ShowingDialog());
        
        dialogManager.enabled = false;

        currentDialogIndex++;

        NextImage();
    }

    private IEnumerator WaitAtImage()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && canSkip);

        canSkip = false;

        NextImage();
    }

    private void NextImage()
    {
        currentImageIndex++;

        if (currentImageIndex >= imageLeadsTo.Count || currentImageIndex >= images.Count)
            SceneManager.LoadScene(nextScene);

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
}