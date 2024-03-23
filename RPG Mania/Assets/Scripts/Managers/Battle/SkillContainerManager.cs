using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillContainerManager : MonoBehaviour {
    [SerializeField] private GameObject skillObjectPrefab;
    [SerializeField] private MenuContainer menuContainer;
    [SerializeField] private BattleUIManager uiManager;

    private List<(GameObject, SkillAction)> skillObjects = new();
    private GameObject absorbObject;
    public SkillAction[] skills;

    public void Initialize() {
        skills = SkillList.GetInstance().GetActions();

        absorbObject = Instantiate(skillObjectPrefab, transform);
        absorbObject.GetComponent<Button>().onClick.AddListener(UseAbsorb);
        absorbObject.GetComponentInChildren<TextMeshProUGUI>().text = "Absorb";

        foreach (SkillAction skill in skills) {
            GameObject skillObj = Instantiate(skillObjectPrefab, transform);
            skillObj.GetComponent<Button>().onClick.AddListener(() => UseSkill(skill));
            skillObj.GetComponentInChildren<TextMeshProUGUI>().text = skill.Name;

            skillObjects.Add((skillObj, skill));
            skillObj.SetActive(false);
        }

        GameObject skillObject = Instantiate(skillObjectPrefab, transform);
        skillObject.GetComponent<Button>().onClick.AddListener(Back);
        skillObject.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
    }
    public void UpdateSkills(PlayerBattle player)
    {
        if (skillObjects.Count == 0) Initialize();

        List<GameObject> activeSkills = new() { absorbObject };

        for (int i = 0; i < skillObjects.Count; i++)
        {
            GameObject skillObject = skillObjects[i].Item1;
            SkillAction skill = skillObjects[i].Item2;

            if (player.CanUseSkill(skill))
            {
                skillObject.SetActive(true);
                activeSkills.Add(skillObject);
            }
            else
            {
                skillObject.SetActive(false);
            }
        }

        menuContainer.SetButtons(activeSkills);
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