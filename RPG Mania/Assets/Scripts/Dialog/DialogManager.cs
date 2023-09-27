using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text dialogName;
    [SerializeField] private float textSpeed = .035f;
    public DialogObject currentDialog;
    private bool showingDialog = true;
    public bool clicked = false;

    public void Start()
    {
        
    }

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
        showingDialog = false;
    }

    public bool ShowingDialog()
    {
        return showingDialog;
    }

    public void DisplayDialog(DialogObject dialogObject)
    {
        showingDialog = true;
        StartCoroutine(MoveThroughDialog(dialogObject));
    }

    private void OnEnable() {
        dialogBox.SetActive(true);
    }

    private void OnDisable() {
        dialogBox.SetActive(false);
    }


    private void Update() {
        if(Input.GetMouseButtonDown(0))
        {
            clicked = true;
        }
    }
}
