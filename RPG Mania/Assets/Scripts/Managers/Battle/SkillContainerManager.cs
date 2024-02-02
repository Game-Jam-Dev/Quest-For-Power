using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillContainerManager : MonoBehaviour {
    [SerializeField] private GameObject skillObjectPrefab;
    [SerializeField] private UIManager uiManager;

    private List<(GameObject, SkillAction)> skillObjects = new();

    public void Initialize() {
        SkillAction[] skills = SkillList.GetInstance().GetActions();

        GameObject skillObj = Instantiate(skillObjectPrefab, transform);
        skillObj.GetComponent<Button>().onClick.AddListener(UseAbsorb);
        skillObj.GetComponentInChildren<TextMeshProUGUI>().text = "Absorb";

        foreach (SkillAction skill in skills) {
            skillObj = Instantiate(skillObjectPrefab, transform);
            skillObj.GetComponent<Button>().onClick.AddListener(() => UseSkill(skill));
            skillObj.GetComponentInChildren<TextMeshProUGUI>().text = skill.Name;

            skillObjects.Add((skillObj, skill));
            skillObj.SetActive(false);
        }

        skillObj = Instantiate(skillObjectPrefab, transform);
        skillObj.GetComponent<Button>().onClick.AddListener(Back);
        skillObj.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
    }
    public void UpdateSkills(PlayerBattle player)
    {
        if (skillObjects.Count == 0) Initialize();

        for (int i = 0; i < skillObjects.Count; i++)
        {
            GameObject skillObject = skillObjects[i].Item1;
            SkillAction skill = skillObjects[i].Item2;

            if (player.CanUseSkill(skill))
            {
                skillObject.SetActive(true);
            }
            else
            {
                skillObject.SetActive(false);
            }
        }
    }

    private void UseSkill(SkillAction skill)
    {
        uiManager.PickSkill(skill);
    }

    private void UseAbsorb()
    {
        uiManager.PickAbsorb();
    }

    private void Back()
    {
        uiManager.BackFromSkill();
    }
}