using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class BattleManager : MonoBehaviour {
    [Header("Managers")]
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private BattleUIManager battleUIManager;

    [Header("Other Variables")]
    public float dialogueDisplayTime = 1.5f;

    [Header("Extras, no need to change")]
    public List<EnemyInfo> enemies = new();
    private Queue<CharacterInfo> turnOrder = new();
    public List<ComboAction> activeCharacterCombo = new();

    // character that will be attacked
    public CharacterInfo target;
    private PlayerInfo player;
    private Coroutine battleLoop;

    // tracking variables
    public int killCount = 0;
    private int xpGain = 0;
    public bool awaitCommand = false;
    private int comboLength = 0;
    public bool absorb = false;
    public int activeSkillCounter = 0;

    private void Awake() {
        gameObject.SetActive(false);
    }
    
    private void OnEnable() {
        // Setup Characters
        InitializeCharacters();

        // Setup UI
        SetMenus();

        // start the battle loop
        battleLoop = StartCoroutine(BattleSequence());
    }

    private void InitializeCharacters()
    {
        // set enemies, end battle if none
        enemies = worldManager.GetBattleEnemies();
        if (enemies.Count <= 0) EndBattle();

        // save player reference
        player = worldManager.GetPlayer();

        // Set up the turn order
        IEnumerable<CharacterInfo> characters = new List<CharacterInfo>{ player }.Concat(enemies);
        worldManager.PrepareCharactersForCombat(characters);
        turnOrder = new Queue<CharacterInfo>(characters);
    }

    private void SetMenus()
    {
        battleUIManager.SetForBattle(player, enemies);
    }

    private IEnumerator BattleSequence()
    {
        while (true) {
            if (turnOrder.Count > 0)
            {
                CharacterInfo activeCharacter = turnOrder.Peek();

                if (activeCharacter == player)
                {

                    // prepare variables for the players turn
                    InitializePlayer();

                    // activate ui
                    battleUIManager.ActivateForPlayerTurn();
                    
                    // make the loop stay here until the player selects their commands in the ui
                    while (awaitCommand)
                    {

                        yield return null;
                    }

                    // do the absorb action
                    if (absorb)
                    {
                        Absorb(activeCharacter);
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
                            target.Recover();

                            // handles a miss
                            if (!hit)
                            {
                                battleUIManager.SetText($"{activeCharacter.characterName} missed");
                                break;
                            }

                            // update enemy's health ui
                            battleUIManager.UpdateEnemyHealth();

                            // handle enemy's death
                            if (target.health <= 0)
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
                    EnemyComboCreation(activeCharacter as EnemyInfo);

                    // enemy targets the player
                    target = player;

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
                        target.Recover();

                        // handles a miss
                        if (!hit)
                        {
                            battleUIManager.SetText($"{activeCharacter.characterName} missed");
                            break;
                        }

                        // updates player's health
                        battleUIManager.UpdatePlayerHealth();

                        // lose battle if player dies
                        if (target.health <= 0)
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

            // yield return null;
        }
    }

    private void InitializePlayer()
    {
        // Prepares player for turn
        awaitCommand = true;
        
    }

    private void NextTurn(CharacterInfo activeCharacter)
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
        StopCoroutine(battleLoop);
        worldManager.EscapeBattle();
        EndBattle();
    }

    private bool Attack(CharacterInfo activeCharacter, ComboAction comboAction, int hitNumber = 0)
    {
        battleUIManager.SetText($"{activeCharacter.characterName} used {comboAction.Name} at {target.characterName}");

        return activeCharacter.DoAction(comboAction, target, hitNumber);
    }

    private void Absorb(CharacterInfo activeCharacter)
    {
        (activeCharacter as PlayerInfo).AbsorbSkill(target.element);

        battleUIManager.SetText($"{player.characterName} absorbed the {target.element} element from {target.characterName}");

        battleUIManager.UpdatePlayerHealth();
        battleUIManager.UpdateSkills();
    }

    private void DefeatedEnemy()
    {
        // convert target into the EnemyInfo type
        EnemyInfo defeatedEnemy = target as EnemyInfo;

        // take enemy out of battle system
        var newTurnOrder = new Queue<CharacterInfo>(turnOrder.Where(x => x != defeatedEnemy));
        turnOrder = newTurnOrder;

        // gain stats from kill
        killCount++;
        xpGain += defeatedEnemy.XPFromKill(player.level);

        // remove enemy from ui
        battleUIManager.DefeatedEnemy(defeatedEnemy);
        defeatedEnemy.Defeated();

        // end battle if it was the last enemy
        // switch if statements if battle ends incorrectly
        // if (killCount >= enemies.Count) 
        if (turnOrder.Count <= 1)
            StartCoroutine(WinBattle());
    }

    private void EnemyComboCreation(EnemyInfo enemy)
    {
        // runs until enemy fills combo 
        while (comboLength < enemy.combo)
        {
            ComboAction comboAction = enemy.PickEnemyCombo(comboLength);
            activeCharacterCombo.Add(comboAction);
            comboLength += comboAction.Cost;
        }
    }

    public void SetComboAction(CharacterInfo target, List<ComboAction> activeCharacterCombo)
    {
        this.target = target;
        this.activeCharacterCombo = activeCharacterCombo;
        awaitCommand = false;
    }

    public void SetAbsorbAction(CharacterInfo target)
    {
        this.target = target;
        absorb = true;
        awaitCommand = false;
    }

    public void UpdateSkillCounter()
    {
        if (player.activeSkill != null)
        {
            if (activeSkillCounter < 3)
            {
                if (activeSkillCounter == 0)
                    player.LoseSkillUse();

                activeSkillCounter++;
            }
            else 
            {
                player.DeactivateSkill();
                activeSkillCounter = 0;
                battleUIManager.ClearActiveSkill();
            }
        }
    }

    public void UnselectSkill()
    {
        if (activeSkillCounter == 0)
        {
            player.DeactivateSkill();

            battleUIManager.ClearActiveSkill();
        }
    }

    private void EndBattle()
    {
        player.EndCombat();

        StopAllCoroutines();

        enemies.Clear();

        xpGain = 0;
        killCount = 0;
        comboLength = 0;
        activeCharacterCombo.Clear();
        absorb = false;

        battleUIManager.ClearUI();

        gameObject.SetActive(false);
    }

    private IEnumerator WinBattle()
    {
        StopCoroutine(battleLoop);

        battleUIManager.SetText("You won!");

        int currentLevel = player.level;

        player.WinBattle(xpGain, killCount);

        yield return new WaitForSeconds(dialogueDisplayTime);

        if (player.level > currentLevel)
        {
            battleUIManager.SetText("Level up! You are now level " + player.level);
            yield return new WaitForSeconds(dialogueDisplayTime);
        }

        foreach (EnemyInfo e in enemies)
        {
            e.Kill();
        }

        worldManager.WinBattle();

        EndBattle();
    }

    private IEnumerator LoseBattle()
    {
        StopCoroutine(battleLoop);

        battleUIManager.SetText("You were defeated!");

        yield return new WaitForSeconds(dialogueDisplayTime);

        worldManager.LoseBattle();
    }
}