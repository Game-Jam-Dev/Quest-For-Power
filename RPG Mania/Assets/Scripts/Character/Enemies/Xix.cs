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
        maxHealth = 50 + level * 5;
        attack = 8 + (int)(level * .8f);
        defense = 7 + (int)(level * 1.2f);
        accuracy = .65f + level / 3500f;
        evasion = .01f + level / 1000f;

        combo = 3 + (int)Mathf.Pow(level, .25f);

        health = maxHealth;
    }

    public override int XPFromKill(int playerLevel)
    {
        int xp = playerLevel * 20;

        return xp;
    }

    protected override void OnTriggerEnter(Collider other) {
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