using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillContainerManager : MonoBehaviour {
    [SerializeField] private GameObject skillObjectPrefab;
    [SerializeField] private UIManager uiManager;

    private List<(GameObject, SkillAction)> skillObjects = new();

    private void Start() {
        SkillAction[] skills = SkillList.GetInstance().GetActions();

        foreach (SkillAction skill in skills) {
            GameObject skillObject = Instantiate(skillObjectPrefab, transform);

            skillObject.GetComponent<Button>().onClick.AddListener(() => UseSkill(skill));
            skillObject.GetComponentInChildren<TextMeshProUGUI>().text = skill.Name;
            skillObjects.Add((skillObject, skill));
            skillObject.SetActive(false);
        }
    }
    public void UpdateSkills(PlayerBattle player)
    {
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
}