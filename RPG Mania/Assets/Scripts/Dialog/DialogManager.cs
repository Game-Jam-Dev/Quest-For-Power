using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text dialogName;
    [SerializeField] private float textSpeed;
    public DialogObject currentDialog;

    public void Start()
    {
        DisplayDialog(currentDialog);
    }

    private IEnumerator MoveThroughDialog(DialogObject dialogObject)
    {
        for(int i = 0; i < dialogObject.dialogLines.Length; i++)
        {
            dialogText.text = "";
            dialogName.text = dialogObject.dialogLines[i].speakerName;
            foreach (char c in dialogObject.dialogLines[i].dialog)
            {
                //if (dialogText.text == dialogObject.dialogLines[i].dialog) { break; }
                dialogText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            //The following line of code makes it so that the for loop is paused until the user clicks the left mouse button.
            yield return new WaitUntil (()=>Input.GetMouseButtonDown(0));
            //The following line of code makes the coroutine wait for a frame so as the next WaitUntil is not skipped
            yield return null;
        }
        dialogBox.SetActive(false);
    }

    public void DisplayDialog(DialogObject dialogObject)
    {
        StartCoroutine(MoveThroughDialog(dialogObject));
    }
}
