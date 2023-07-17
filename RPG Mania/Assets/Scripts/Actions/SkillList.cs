using System.Collections.Generic;
using UnityEngine;

public class SkillList
{
    public static SkillList Instance { get; private set; }
    private IDictionary<string, SkillAction> skillList;

    private SkillList()
    {
        FillDictionary();
    }

    public static SkillList GetInstance()
    {
        if (Instance == null)
        {
            Instance = new SkillList();
        }

        return Instance;
    }

    public SkillAction GetAction(string key)
    {
        if (skillList.ContainsKey(key)) return skillList[key];

        else return new SkillAction("Null", 0, EmptyAction);
    }


    private void FillDictionary()
    {
        skillList = new Dictionary<string, SkillAction>()
        {
            {"health drain", new SkillAction("Health Drain", 5, HealthDrain)}
        };
    }

    public void EmptyAction(CharacterInfo self, CharacterInfo target, int damage)
    {
        Debug.Log("This action is null");
    }

    public void HealthDrain(CharacterInfo self, CharacterInfo target, int damage)
    {
        self.health += damage/2;

        if (self.health > self.maxHealth) self.health = self.maxHealth;
    }
}