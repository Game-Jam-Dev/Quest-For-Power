using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Utility {
    
    public static void SetActiveButton(Button button)
    {
        if (button != null)
            EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public static bool CheckIfAnimationParamExists(string triggerName, Animator anim) 
    {
        if (anim == null) return false;

        int hash = Animator.StringToHash(triggerName);
        for (int i = 0; i < anim.parameterCount; i++)
        {
            AnimatorControllerParameter param = anim.GetParameter(i);
            if (param.nameHash == hash)
                return true;
        }
        return false;
    }

    public static int CountListOfInts(List<int> ints)
    {
        int c = 0;

        foreach (int i in ints)
        {
            c += i;
        }

        return c;
    }

    public static void SwitchActiveObjects(GameObject previous, GameObject next)
    {
        previous.SetActive(false);
        next.SetActive(true);
    }

    public static string FormatTimeToString(float time)
    {
        string numberDivider = ":";

        string hours = ((int)time / 3600).ToString("00");
        string minutes = ((int)time % 3600 / 60).ToString("00");
        string seconds = ((int)time % 3600 % 60).ToString("00");

        return hours + numberDivider + minutes + numberDivider + seconds;
    }

    public static IEnumerator WaitAFrame()
    {
        yield return new WaitForEndOfFrame();
    }
}