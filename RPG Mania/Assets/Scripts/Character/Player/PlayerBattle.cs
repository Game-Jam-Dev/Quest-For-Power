using UnityEngine;
using System.Collections.Generic;

public class PlayerBattle : CharacterBattle {
    public int experience = 0;
    public int level { get; private set; }


    [SerializeField] private PlayerAnimation pa;
    [SerializeField] private AudioClip waterClip, fireClip, windClip, earthClip, drainClip;
    [SerializeField] public int baseAttack = 6, baseMaxHealth = 60, baseDefense = 5, 
        baseComboPoints = 4;
    [SerializeField] public float baseAccuracy = .975f, baseEvasion = 0.025f;

    private List<(AudioClip, float)> elementClips;

    public int extraComboPoints;
    public int characterComboPoints;

    static string characterName = "Arkanos";
    [SerializeField] float bonusAttackPercentage = .175f, bonusDefensePercentage = .35f, bonusAccuracy = 0.09f, bonusEvasion = 0.09f;
    bool fireBonus = false, waterBonus = false, windBonus = false, earthBonus = false;
    private float oldValue1, oldValue2;
    protected override void Start() {
        base.Start();

        skillKeys.Add("health drain");
        skillKeys.Add("water");
        skillKeys.Add("fire");
        skillKeys.Add("wind");
        skillKeys.Add("earth");

        List<int> playerSkillUses = GameManager.instance.GetPlayerSkillUses();
        SkillList skillList = SkillList.GetInstance();

        for (int i = 0; i < skillKeys.Count; i++)
        {
            skillActions.Add((skillList.GetAction(skillKeys[i]), playerSkillUses[i]));
        }

        elementClips = new List<(AudioClip, float)>() { (drainClip, 2.45f), (waterClip, 1.03f), (fireClip, .7f), (windClip, 2.2f), (earthClip, .93f) };
    }

    public override void PrepareCombat()
    {
        pa.SwitchToCombat();
        GetComponent<PlayerMovement>().enabled = false;
        combo = characterComboPoints;
    }

    public void AddExtraComboPoints(int extraPoints)
    {
        extraComboPoints += extraPoints;
        Debug.Log("Adding extra points: " + extraComboPoints);
    }

    public void ResetExtraComboPoints()
    {
        extraComboPoints = 0;
        Debug.Log("Reseting extra points: " + extraComboPoints);
    }

    public void UpdateComboPoints()
    {
        if (element == ElementManager.Element.Wind)
        {
            extraComboPoints += 3;
        }
        combo = characterComboPoints + (int)Mathf.Round(extraComboPoints / 3f);
        if (combo > 1.5f * characterComboPoints)
        {
            combo = (int)Mathf.Round(1.5f * characterComboPoints);
        }
        Debug.Log("Updating combo:" + combo.ToString() + " character base points: " + characterComboPoints +
            " extra points: " + extraComboPoints);
    }

    public void WinBattle(int xp, int kills, List<Item> itemDrops)
    {
        int xpForLevel = XpForLevel();

        experience += xp;

        if (experience >= xpForLevel) LevelUp(xpForLevel);

        GameManager.instance.SetPlayerExperience(experience);

        GameManager.instance.AddItems(itemDrops);

        SaveSystem.SaveGameData(GameManager.instance.GetGameData());
        combo = characterComboPoints;
    }

    public int XpForLevel()
    {
        return 10 + (int)Mathf.Pow(level + 1, 2.5f);
    }

    private void LevelUp(int xpForLevel)
    {
        // experience -= xpForLevel;
        level++;

        SetStats(level);

        GameManager.instance.SetPlayerLevel(level);

        ResetHealth();
    }

    public void SetStats(int level)
    {
        // Old stats:
        //maxHealth = 20 + level * 5;
        //attack = 9 + (int)(level * 1.5f);
        //defense = 5 + (int)(level * 1.1f);
        //accuracy = .85f + level / 2000f;
        //evasion = .05f + level / 1000f;
        //combo = 3 + (int)Mathf.Pow(level, .3f);
        //baseAttack, baseMaxHealth, baseDefense, baseAccuracy, baseEvasion, baseComboPoints

        maxHealth = baseMaxHealth + level * 5;
        attack = baseAttack + (int)(level * 1.5f);
        defense = baseDefense + (int)(level * 1.1f);
        accuracy = baseAccuracy + level / 2000f;
        evasion = baseEvasion + level / 1000f;

        characterComboPoints = baseComboPoints + (int)Mathf.Log(.475f * level) +
            (int).02f * level;
    }

    public void EndCombat()
    {
        pa.SwitchFromCombat();

        DeactivateSkill();

        GetComponent<PlayerMovement>().enabled = true;

        SetStats(level);
    }

    public override void SetAnimationTrigger(string triggerName)
    {
        pa.SetUpTrigger(triggerName);
    }

    public void SetData(int level, int experience, List<int> skillUses)
    {
        this.level = level;
        this.experience = experience;

        SetStats(level);

        for (int i = 0; i < skillActions.Count; i++)
        {
            skillActions[i] = (skillActions[i].Item1, skillUses[i]);
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public override void SelectSkill(SkillAction skill)
    {
        activeSkill = skill;
        //check if the interactions with items may break these calculations
        if (activeSkill == SkillList.GetInstance().GetAction("water"))
        {
            element = ElementManager.Element.Water;

            ResetElementBonus();
            oldValue1 = accuracy;
            oldValue2 = evasion;
            accuracy += bonusAccuracy;
            evasion += bonusEvasion;
            waterBonus = true;

        } else if (activeSkill == SkillList.GetInstance().GetAction("fire"))
        {
            element = ElementManager.Element.Fire;

            ResetElementBonus();
            oldValue1 = attack;
            attack += (int)Mathf.Round(bonusAttackPercentage * attack);
            fireBonus = true;

        } else if (activeSkill == SkillList.GetInstance().GetAction("wind"))
        {
            element = ElementManager.Element.Wind;

            ResetElementBonus();
            windBonus = true;

        } else if (activeSkill == SkillList.GetInstance().GetAction("earth"))
        {
            element = ElementManager.Element.Earth;

            ResetElementBonus();
            oldValue1 = defense;
            defense += (int)Mathf.Round(bonusDefensePercentage * defense);
            earthBonus = true;

        } else
        {
            element = ElementManager.Element.None;

            ResetElementBonus();

        }

        pa.SetElement(element);

        audioSource.clip = elementClips[(int)element].Item1;
        audioSource.time = elementClips[(int)element].Item2;
        audioSource?.Play();
    }

    public void ResetElementBonus()
    {
        if (waterBonus)
        {
            accuracy = oldValue1;
            evasion = oldValue2;
            waterBonus = false;
        }
        if (fireBonus)
        {
            attack = (int)oldValue1;
            fireBonus = false;
        }
        if (windBonus)
        {
            windBonus = false;
        }
        if (earthBonus)
        {
            defense = (int)oldValue1;
            earthBonus = false;
        }
    }

    public bool CanUseSkill(SkillAction skill)
    {
        int index = skillActions.FindIndex(item => item.Item1 == skill);

        if (index != -1) return GameManager.instance.GetPlayerSkillUses()[index] > 0;

        else return false;
    }

    public int GetSkillAmount(SkillAction skill)
    {
        int index = skillActions.FindIndex(item => item.Item1 == skill);

        return skillActions[index].Item2;
    }

    public void AbsorbSkill(ElementManager.Element e)
    {
        int index = (int)e;
        int spellsTaken = Random.Range(1,4);

        skillActions[index] = (skillActions[index].Item1, skillActions[index].Item2 + spellsTaken);

        audioSource.clip = elementClips[0].Item1;
        audioSource.time = elementClips[0].Item2;
        audioSource.Play();

        GameManager.instance.SetPlayerSkill(index, skillActions[index].Item2);

        Heal(maxHealth/2);
    }

    public void LoseSkillUse(int n = 1)
    {
        int index = skillActions.FindIndex(item => item.Item1 == activeSkill);

        if (index != -1)
        {
            int newCount = skillActions[index].Item2 - n;
            if (newCount < 0) newCount = 0;
            skillActions[index] = (skillActions[index].Item1, newCount);

            GameManager.instance.SetPlayerSkill(index, skillActions[index].Item2);
        }
    }

    public void DeactivateSkill()
    {
        element = ElementManager.Element.None;
        activeSkill = null;
        pa.SetElement(element);
    }

    public override Animator GetAnimator() {return pa.GetAnimator();}

    public PlayerAnimation GetPlayerAnimation() { return pa; }

    public override bool GetIsAttacking() {return pa.isAttacking;}
}