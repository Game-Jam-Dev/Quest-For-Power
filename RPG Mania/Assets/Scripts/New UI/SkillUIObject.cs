using UnityEngine;
using System.Text.RegularExpressions;

public class SkillUIObject : MonoBehaviour
{
    public string spellDescription;
    string spellName;

    [SerializeField]
    GameObject spellDescriptionBox;

    //private void Start()
    //{
    //    spellDescriptionBox = GameObject.Find("SpellDescription");
    //}

    public void ClickOnText()
    {
        spellName = Regex.Split(GetComponent<TMPro.TextMeshProUGUI>().text, "\r\n")[0];
        //Debug.Log("This item name: " + itemName);
        //Debug.Log(itemName.Length);
        SkillAction[] skills = SkillList.GetInstance().GetActions();
        //Debug.Log(items);
        foreach (SkillAction skill in skills)
        {
            //Debug.Log("Item list object: " + item.itemName);
            //Debug.Log(item.itemName.Length);
            //Debug.Log(item.itemName == itemName);
            if (skill.Name == spellName)
            {
                //Debug.Log("Found match");
                spellDescription = skill.Description;
                Debug.Log(spellDescription);
                spellDescriptionBox.GetComponent<TMPro.TextMeshProUGUI>().text = spellDescription;
                break;
            }
        }
    }
}
