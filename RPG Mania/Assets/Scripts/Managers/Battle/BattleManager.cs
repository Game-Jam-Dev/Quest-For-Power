using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

public class BattleManager : MonoBehaviour {
    [Header("Managers")]
    [SerializeField] public WorldManager worldManager;
    [SerializeField] private BattleUIManager battleUIManager;

    [Header("Other Variables")]
    public float dialogueDisplayTime = 1.5f;

    [Header("Extras, no need to change")]
    public List<EnemyBattle> enemies = new();
    public PlayerBattle player;
    private Item item;
    private Queue<CharacterBattle> turnOrder = new();
    public CharacterBattle characterToAttack;
    public EnemyBattle enemyToAttack;
    public List<ComboAction> activeCharacterCombo = new();
    private Coroutine battleLoop;

    // tracker variables
    public int killCount = 0;
    private int xpGain = 0;
    public List<Item> itemDrops = new();
    public bool awaitCommand = false;
    private int comboLength = 0;
    public bool absorb = false;
    public int activeSkillCounter = 0;

    GameObject combatResolutionUI;
    EnemyBattle defeatedEnemy;

    Vector3 originalPosition = Vector3.zero;
    List<EnemyBattle> battleEnemies;

    private void Awake() {
        gameObject.SetActive(false);
        combatResolutionUI = GameObject.Find("CombatResolution");
    }
    
    private void OnEnable() 
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.DisablePausing();
        }

        // Setup Characters
        InitializeCharacters();

        // Setup UI
        SetUpUI();

        // start the battle loop
        battleLoop = StartCoroutine(BattleLoop());
    }

    private void OnDisable() 
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.EnablePausing();
        }
    }

    private void InitializeCharacters()
    {
        // set enemies, end battle if none
        // set enemies to a copy of the list to avoid modifying the original
        

        battleEnemies = worldManager.GetBattleEnemies();
        foreach (EnemyBattle e in battleEnemies)
        {
            enemies.Add(e);
        }

        if (enemies.Count <= 0) EndBattle();

        // save player reference
        player = worldManager.GetPlayer();

        // Set up the turn order
        IEnumerable<CharacterBattle> characters = new List<CharacterBattle>{ player }.Concat(enemies);
        worldManager.PrepareCharactersForCombat(characters);
        turnOrder = new Queue<CharacterBattle>(characters);
    }

    private void SetUpUI()
    {
        battleUIManager.Initialize(player, enemies);
    }

    private IEnumerator BattleLoop()
    {
        // Slow down game
        //Time.timeScale = 0.1f;

        while (turnOrder.Count > 0) {
            // get next character in order
            CharacterBattle activeCharacter = turnOrder.Peek();

            if (activeCharacter == player)
            {
                // prepare variables for the players turn
                InitializePlayer();

                // activate ui
                battleUIManager.StartPlayerTurn();
                
                // make the loop stay here until the player selects their commands in the ui
                while (awaitCommand)
                {

                    yield return null;
                }

                while (!CheckEnemiesReady())
                    yield return null;

                // do the absorb action
                if (absorb)
                {
                    Absorb(activeCharacter);

                    while (activeCharacter.GetIsAttacking())
                    {
                        
                        yield return null;
                    }
                } else if (item != null)
                {
                    ItemManager.UseItem(item, this);

                    battleUIManager.SetText($"{player.characterName} used {item.itemName}");

                    battleUIManager.UpdatePlayerHealth();
                    battleUIManager.UpdateItems();

                    if (player.health <= 0)
                    {
                        StartCoroutine(LoseBattle());

                        break;
                    }

                    if (characterToAttack != null)
                    {
                        battleUIManager.UpdateEnemyHealth((EnemyBattle)characterToAttack);

                        // handle enemy's death
                        if (characterToAttack.health <= 0)
                        {
                            DefeatedEnemy();
                        }
                    }
                }
                // standard action
                else
                {
                    originalPosition = activeCharacter.transform.position;
                    //(activeCharacter as PlayerBattle).ToggleNormalAttack(characterToAttack.transform.position, false);
                    // track the index of the attack in the combo
                    int i = 0;

                    int used_combo_points = 0;
                    foreach (ComboAction a in activeCharacterCombo)
                    {
                        // do the attack and save whether it hit or not
                        bool hit = Attack(activeCharacter, a, i);

                        used_combo_points += a.Cost;

                        // wait for the attack animation to play
                        while (!CheckEnemiesReady() || !(activeCharacter as PlayerBattle).CheckPlayerReady())
                        {

                            yield return null;
                        }

                        // attacked character resets
                        //characterToAttack.Recover();

                        // handles a miss
                        if (!hit)
                        {
                            battleUIManager.SetText($"{activeCharacter.characterName} missed");
                            //break;
                        }

                        // update enemy's health ui
                        battleUIManager.UpdateEnemyHealth((EnemyBattle)characterToAttack);

                        // handle enemy's death
                        if (characterToAttack.health <= 0)
                        {
                            // refund remaining combo points if enemy died
                            player.AddExtraComboPoints(player.combo - used_combo_points);

                            DefeatedEnemy();

                            // break out of combo when enemy dies
                            break;

                            
                        }
                        i++;
                    }
                    //(activeCharacter as PlayerBattle).ToggleNormalAttack(originalPosition, true);
                    (activeCharacter as PlayerBattle).UpdateComboPoints();
                }
                UpdateSkillCounter();
            } else {
                EnemyBattle activeEnemy = (EnemyBattle)activeCharacter;
                if (!activeEnemy.stunned)
                {
                    while (!CheckEnemiesReady())
                        yield return null;
                    // set enemy combo
                    EnemyComboCreation(activeCharacter as EnemyBattle);

                    // enemy targets the player
                    characterToAttack = player;

                    // track the index of the attack in the combo
                    int i = 0;
                    foreach (ComboAction a in activeCharacterCombo)
                    {
                        // do the attack and save whether it hit or not
                        bool hit = Attack(activeCharacter, a, i);

                        // wait for the attack animation to play
                        // while (!activeEnemy.GetIsReady())
                        // {

                        //     yield return null;
                        // }

                        // attacked character resets
                        //characterToAttack.Recover();

                        // handles a miss
                        if (!hit)
                        {
                            battleUIManager.SetText($"{activeCharacter.characterName} missed");
                            //break;
                        }
                        else
                            (characterToAttack as PlayerBattle).PlayDamagedAnimation();

                        // updates player's health
                        battleUIManager.UpdatePlayerHealth();

                        while (!CheckEnemiesReady())
                            yield return null;

                        // lose battle if player dies
                        if (characterToAttack.health <= 0)
                        {
                            StartCoroutine(LoseBattle());

                            break;
                        }
                        i++;
                    }
                }
            }

            // creates a pause between turns
            yield return new WaitForSeconds(.5f);

            NextTurn(activeCharacter);
        }
    }

    private void InitializePlayer()
    {
        // Prepares player for turn
        awaitCommand = true;
        item = null;
    }

    private void NextTurn(CharacterBattle activeCharacter)
    {
        // moves character that just acted to the back
        turnOrder.Dequeue();
        turnOrder.Enqueue(activeCharacter);

        // reset necessary variables
        activeCharacterCombo.Clear();
        comboLength = 0;
        absorb = false;
    }

    public void Escape()
    {
        bool canEscape = worldManager.canEscape;

        if (!canEscape)
        {
            battleUIManager.SetText("You can't escape!");
            return;
        }

        StopCoroutine(battleLoop);
        worldManager.EscapeBattle();
        EndBattle();
        battleUIManager.Escape();
    }

    private bool Attack(CharacterBattle activeCharacter, ComboAction comboAction, int hitNumber = 0)
    {
        
        if (activeCharacter.characterName == "Arkanos")
        {
            (activeCharacter as PlayerBattle).PlayAttackAnimation(comboAction);
            battleUIManager.SetText($"{activeCharacter.characterName} used {comboAction.Name} at {characterToAttack.characterName}");
            if (hitNumber == 0 & comboAction.Cost == enemyToAttack.firstComboValue) 
            {
                enemyToAttack.ReduceShields();
                enemyToAttack.PlayAttackedAnimation();
                (activeCharacter as PlayerBattle).AddExtraComboPoints(comboAction.Cost);
            }
            else if (hitNumber == 1 & comboAction.Cost == enemyToAttack.secondComboValue)
            {
                enemyToAttack.ReduceShields();
                enemyToAttack.PlayAttackedAnimation();
                (activeCharacter as PlayerBattle).AddExtraComboPoints(comboAction.Cost);
            }
            else if (hitNumber == 2 & comboAction.Cost == enemyToAttack.thirdComboValue)
            {
                enemyToAttack.ReduceShields();
                enemyToAttack.PlayAttackedAnimation();
                (activeCharacter as PlayerBattle).AddExtraComboPoints(comboAction.Cost);
            }
            else
            {
                enemyToAttack.PlayBlockAnimation();
            }
            //(activeCharacter as PlayerBattle).SetAnimationTrigger("Jump");
            //(activeCharacter as PlayerBattle).SetAnimationTrigger(comboAction.Name);
        }
        else
        {
            enemyToAttack = (EnemyBattle)activeCharacter;
            if (!enemyToAttack.stunned)
            {
                (activeCharacter as EnemyBattle).PlayAttackAnimation();
                battleUIManager.SetText($"{activeCharacter.characterName} used {comboAction.Name} at {characterToAttack.characterName}");
            }            
        }

        return activeCharacter.DoAction(comboAction, characterToAttack, hitNumber);
    }

    private void Absorb(CharacterBattle activeCharacter)
    {
        (activeCharacter as PlayerBattle).AbsorbSkill(characterToAttack.element);


        battleUIManager.SetText($"{player.characterName} absorbed the {characterToAttack.element} element from {characterToAttack.characterName}");

        battleUIManager.UpdatePlayerHealth();
        battleUIManager.UpdateSkills();
    }

    private void DefeatedEnemy()
    {
        // convert target into the EnemyInfo type
        defeatedEnemy = characterToAttack as EnemyBattle;

        // take enemy out of battle system
        var newTurnOrder = new Queue<CharacterBattle>(turnOrder.Where(x => x != defeatedEnemy));
        turnOrder = newTurnOrder;

        // gain stats from kill
        killCount++;
        xpGain += defeatedEnemy.XPFromKill(player.level);

        if (defeatedEnemy.itemDrops) itemDrops.Add(defeatedEnemy.itemDrop);

        // remove enemy from ui
        battleUIManager.DefeatedEnemy(defeatedEnemy);
        defeatedEnemy.Defeated();

        battleUIManager.SetText($"{defeatedEnemy.characterName} was defeated!");

        // end battle if it was the last enemy
        // switch if statements if battle ends incorrectly
        // if (killCount >= enemies.Count) 
        if (turnOrder.Count <= 1)
            StartCoroutine(WinBattle());
    }

    private void EnemyComboCreation(EnemyBattle enemy)
    {
        // runs until enemy fills combo 
        while (comboLength < enemy.combo)
        {
            ComboAction comboAction = enemy.PickEnemyCombo(comboLength);
            activeCharacterCombo.Add(comboAction);
            comboLength += comboAction.Cost;
        }
    }

    public void SetComboAction(CharacterBattle characterToAttack, List<ComboAction> activeCharacterCombo, 
        EnemyBattle targetEnemyClass)
    {
        this.characterToAttack = characterToAttack;
        this.enemyToAttack = targetEnemyClass;
        this.activeCharacterCombo = activeCharacterCombo;
        awaitCommand = false;
    }

    public void SetAbsorbAction(CharacterBattle characterToAttack)
    {
        this.characterToAttack = characterToAttack;
        absorb = true;
        awaitCommand = false;
    }

    public void SetItemAction(Item item, CharacterBattle characterToAttack)
    {
        this.item = item;
        this.characterToAttack = characterToAttack;
        awaitCommand = false;
    }

    public void UpdateSkillCounter()
    {
        // tracks how long the player has been using the same skill
        if (player.activeSkill != null)
        {
            // if less than 3 turns, count up
            if (activeSkillCounter < 3)
            {
                // uses one of the player's uses of skill after first full turn
                if (activeSkillCounter == 0)
                    player.LoseSkillUse();

                activeSkillCounter++;
            }
            // deactivate once time is up
            else 
            {
                player.DeactivateSkill();
                activeSkillCounter = 0;
                battleUIManager.DisableActiveSkill();
            }
        }
    }

    // public void UnselectSkill()
    // {
    //     if (activeSkillCounter == 0)
    //     {
    //         player.DeactivateSkill();

    //         battleUIManager.ClearActiveSkill();
    //     }
    // }

    public Boolean CheckEnemiesReady()
    {
        foreach (EnemyBattle enemy in enemies) 
        { 
            if (!enemy.GetIsReady())
            {
                return false;
            }
        }
        return true;
    }

    public void EndBattle()
    {
        player.EndCombat();

        enemies.Clear();

        xpGain = 0;
        itemDrops.Clear();
        killCount = 0;
        comboLength = 0;
        activeCharacterCombo.Clear();
        absorb = false;
    }

    private void Resolution()
    { 
        StopAllCoroutines();

        battleUIManager.EndBattle();
    }

    private IEnumerator WinBattle()
    {
        StopCoroutine(battleLoop);

        battleUIManager.SetText("You won!");

        int currentLevel = player.level;

        player.WinBattle(xpGain, killCount, itemDrops);

        yield return new WaitForSeconds(dialogueDisplayTime);

        if (player.level > currentLevel)
        {
            battleUIManager.SetText("Level up! You are now level " + player.level);
            yield return new WaitForSeconds(dialogueDisplayTime);
        }

        foreach (EnemyBattle e in enemies)
        {
            e.Kill();
        }

        worldManager.WinBattle();

        Resolution();
    }

    private IEnumerator LoseBattle()
    {
        StopCoroutine(battleLoop);

        player.PlayDeathAnimation();

        battleUIManager.SetText("You were defeated!");

        yield return new WaitForSeconds(dialogueDisplayTime);

        worldManager.LoseBattle();
    }
}