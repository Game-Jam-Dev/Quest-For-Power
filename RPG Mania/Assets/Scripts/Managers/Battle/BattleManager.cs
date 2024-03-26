using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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

    private void Awake() {
        gameObject.SetActive(false);
        combatResolutionUI = GameObject.Find("CombatResolution");
    }
    
    private void OnEnable() 
    {
        // Setup Characters
        InitializeCharacters();

        // Setup UI
        SetUpUI();

        // start the battle loop
        battleLoop = StartCoroutine(BattleLoop());
    }

    private void InitializeCharacters()
    {
        // set enemies, end battle if none
        enemies = worldManager.GetBattleEnemies();
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

                // do the absorb action
                if (absorb)
                {
                    Absorb(activeCharacter);
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
                    // track the index of the attack in the combo
                    int i = 0;
                    foreach (ComboAction a in activeCharacterCombo)
                    {
                        // do the attack and save whether it hit or not
                        bool hit = Attack(activeCharacter, a, i);
                            
                        // wait for the attack animation to play
                        while (activeCharacter.GetIsAttacking())
                        {
                            
                            yield return null;
                        }

                        // attacked character resets
                        characterToAttack.Recover();

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
                            DefeatedEnemy();

                            // break out of combo when enemy dies
                            break;
                        }
                        i++;
                    }
                }
                UpdateSkillCounter();
            } else {
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
                    while (activeCharacter.GetIsAttacking())
                    {
                        
                        yield return null;
                    }

                    // attacked character resets
                    characterToAttack.Recover();

                    // handles a miss
                    if (!hit)
                    {
                        battleUIManager.SetText($"{activeCharacter.characterName} missed");
                        //break;
                    }

                    // updates player's health
                    battleUIManager.UpdatePlayerHealth();

                    // lose battle if player dies
                    if (characterToAttack.health <= 0)
                    {
                        StartCoroutine(LoseBattle());

                        break;
                    }
                    i++;
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
        if (worldManager is ThroneManager)
        {
            battleUIManager.SetText("You can't run! The kingdom is at stake!");
            return;
        }

        StopCoroutine(battleLoop);
        worldManager.EscapeBattle();
        EndBattle();
    }

    private bool Attack(CharacterBattle activeCharacter, ComboAction comboAction, int hitNumber = 0)
    {
        battleUIManager.SetText($"{activeCharacter.characterName} used {comboAction.Name} at {characterToAttack.characterName}");

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
        EnemyBattle defeatedEnemy = characterToAttack as EnemyBattle;

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

    public void SetComboAction(CharacterBattle characterToAttack, List<ComboAction> activeCharacterCombo)
    {
        this.characterToAttack = characterToAttack;
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

        battleUIManager.SetText("You were defeated!");

        yield return new WaitForSeconds(dialogueDisplayTime);

        worldManager.LoseBattle();
    }
}