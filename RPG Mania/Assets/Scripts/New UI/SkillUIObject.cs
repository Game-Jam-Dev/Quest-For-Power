using UnityEngine;
using System.Text.RegularExpressions;

public class SkillUIObject : MonoBehaviour
{
    public string spellDescription;
    string spellName;

    [SerializeField]
    GameObject spellDescriptionBox;

    public void ClickOnText()
    {
        spellName = Regex.Split(GetComponent<TMPro.TextMeshProUGUI>().text, "\r\n")[0];
        SkillAction[] skills = SkillList.GetInstance().GetActions();
        foreach (SkillAction skill in skills)
        {
            if (skill.Name == spellName)
            {
                spellDescription = skill.Description;
                spellDescriptionBox.GetComponent<TMPro.TextMeshProUGUI>().text = spellDescription;
                break;
            }
        }
    }
}
