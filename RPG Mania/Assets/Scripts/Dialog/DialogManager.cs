using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text dialogName;
    [SerializeField] private float textSpeed = .035f;
    [SerializeField] private Button skipButton;
    private Coroutine currentDialog;
    public bool clicked = false;

    private IEnumerator MoveThroughDialog(DialogObject dialogObject)
    {
        for(int i = 0; i < dialogObject.dialogLines.Length; i++)
        {
            dialogText.text = "";
            dialogName.text = dialogObject.dialogLines[i].speakerName;

            foreach (char c in dialogObject.dialogLines[i].dialog)
            {
                dialogText.text += c;
                yield return new WaitForSeconds(textSpeed);

                // Check if the user has clicked during the text printing
                if (clicked)
                {
                    clicked = false;  // Reset the clicked state
                    dialogText.text = dialogObject.dialogLines[i].dialog; // Show full line
                    break;  // This will break the 'foreach' loop
                }
            }

            //The following line of code makes it so that the for loop is paused until the user clicks the left mouse button.
            yield return new WaitUntil(() => clicked);  // Wait until the Clicked is true
            clicked = false; // Reset the clicked state
            //The following line of code makes the coroutine wait for a frame so as the next WaitUntil is not skipped
            yield return null;
        }

        currentDialog = null;
    }

    public bool ShowingDialog()
    {
        return currentDialog != null;
    }

    public void DisplayDialog(DialogObject dialogObject)
    {
        currentDialog = StartCoroutine(MoveThroughDialog(dialogObject));
    }

    private void OnEnable() {
        dialogBox.SetActive(true);
        Utility.SetActiveButton(skipButton);
    }

    private void OnDisable() {
        dialogBox.SetActive(false);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0) && Time.timeScale == 1)
        {
            clicked = true;
        }
    }

    public void SkipDialog()
    {
        if (currentDialog != null)
        {
            StopCoroutine(currentDialog);
            currentDialog = null;
            dialogText.text = "";
        } 
    }
}
