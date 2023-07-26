using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TMP_Text dialogText;
    public DialogObject currentDialog;

    public void Start()
    {
        DisplayDialog(currentDialog);
    }

    private IEnumerator MoveThroughDialog(DialogObject dialogObject)
    {
        for(int i = 0; i < dialogObject.dialogLines.Length; i++)
        {
            dialogText.text = dialogObject.dialogLines[i].dialog;

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
