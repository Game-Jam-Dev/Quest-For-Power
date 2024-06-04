using UnityEngine;
using System.Collections.Generic;

public class Xix : EnemyBattle {
    private List<EnemyBattle> reinforcements = new List<EnemyBattle>();

    protected override void Start() {
        base.Start();

        ea.AssignElement(element);
    }

    protected override void SetStats()
    {
        maxHealth = 150 + level * 5;
        attack = 7 + (int)(level * .8f);
        defense = 5 + (int)(level * 1.2f);
        accuracy = .7f + level / 3500f;
        evasion = .01f + level / 1000f;

        combo = 3 + (int)Mathf.Pow(level, .25f);

        health = maxHealth;
    }

    public override int XPFromKill(int playerLevel)
    {
        int xp = playerLevel * 20;

        return xp;
    }

    protected override void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetStats();
            wildsManager.BossFight(gameObject);
        }
    }

    public override void AddReinforcement(EnemyBattle e)
    {
        reinforcements.Add(e);
    }
}