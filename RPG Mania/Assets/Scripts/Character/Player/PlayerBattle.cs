using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using System.Collections;

public class PlayerBattle : CharacterBattle {
    public int experience = 0;
    public int level { get; private set; }

    [SerializeField] private AudioClip waterClip, fireClip, windClip, earthClip, drainClip;
    [SerializeField] public int baseAttack = 5, baseMaxHealth = 65, baseDefense = 3, 
        baseComboPoints = 4;
    [SerializeField] public float baseAccuracy = .975f, baseEvasion = .025f;

    private List<(AudioClip, float)> elementClips;

    public int extraComboPoints;
    public int characterComboPoints;

    new static string characterName = "Arkanos";
    [SerializeField] float bonusAttackPercentage = .175f, bonusDefensePercentage = .35f, bonusAccuracy = 0.09f, bonusEvasion = 0.09f;
    bool fireBonus = false, waterBonus = false, windBonus = false, earthBonus = false;
    private float oldValue1, oldValue2;
    
    private CharacterAnimation _pca;

    private void Awake()
    {
        TryGetComponent(out _pca);
    }

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
        GetComponent<PlayerMovement>().enabled = false;
        combo = characterComboPoints;
        DeactivateSkill();
    }

    public void AddExtraComboPoints(int extraPoints)
    {
        extraComboPoints += extraPoints;
    }

    public void ResetExtraComboPoints()
    {
        extraComboPoints = 0;
    }

    public void UpdateComboPoints()
    {
        if (element == ElementManager.Element.Air)
        {
            extraComboPoints += 3;
        }
        combo = characterComboPoints + (int)Mathf.Round(extraComboPoints / 3f);
        if (combo > 1.5f * characterComboPoints)
        {
            combo = (int)Mathf.Round(1.5f * characterComboPoints);
        }
    }

    public void PlayAttackAnimation(ComboAction comboAction)
    {
        _pca.PlayElementBasedCustomThanIdle(comboAction.AnimState);
    }

    public void WinBattle(int xp, int kills, List<Item> itemDrops)
    {
        int xpForLevel = XpForLevel();

        _pca.PlayCustomThanIdle("absorb");

        experience += xp;

        if (experience >= xpForLevel) LevelUp(xpForLevel);

        GameManager.instance.SetPlayerExperience(experience);

        GameManager.instance.AddItems(itemDrops);

        SaveSystem.SaveGameData(GameManager.instance.GetGameData());
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
        DeactivateSkill();

        GetComponent<PlayerMovement>().enabled = true;

        SetStats(level);
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
            element = ElementManager.Element.Air;

            ResetElementBonus();
            UpdateComboPoints();
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

        _pca.AssignElement(element);

        audioSource.clip = elementClips[(int)element].Item1;
        audioSource.time = elementClips[(int)element].Item2;
        audioSource?.Play();
    }

    public void ResetAnimation()
    {
        // Debug.Log("Reseting, element: " + element);
        _pca.Switch2Idle();
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
        
        _pca.AssignElement(ElementManager.Element.None);
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

    public void PlayDamagedAnimation()
    {
        _pca.PlayDamaged();
    }

    public void PlayDeathAnimation() 
    {
        _pca.PlayDeath();
    }

    public void AbsorbSkill(ElementManager.Element e)
    {
        int index = (int)e;
        int spellsTaken = UnityEngine.Random.Range(1,4);

        skillActions[index] = (skillActions[index].Item1, skillActions[index].Item2 + spellsTaken);

        GameManager.instance.SetPlayerSkill(index, skillActions[index].Item2);

        Heal(maxHealth/2);

        combo = characterComboPoints;

        _pca.PlayCustomThanIdle("absorb");
    }

    public void PlayAbsorbSound()
    {
        audioSource.clip = elementClips[0].Item1;
        audioSource.time = elementClips[0].Item2;
        audioSource.Play();
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
        _pca.AssignElement(element);
    }

    public void EscapeFromEnemy()
    {
        float escapeDistanceMultiplier = .2f;

        Transform camera = Camera.main.transform;

        Vector3 direction = camera.position - transform.position;

        transform.position += new Vector3(direction.x, 0, direction.z) * escapeDistanceMultiplier;
    }
}