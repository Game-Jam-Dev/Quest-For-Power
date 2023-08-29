using UnityEngine;
using System.Collections.Generic;

public class XixInfo : EnemyInfo {
    private List<EnemyInfo> reinforcements = new List<EnemyInfo>();

    protected override void Start() {
        base.Start();

        ea.AssignElement(element);
    }

    protected override void SetStats()
    {
        maxHealth = 50 + level * 5;
        attack = 8 + (int)(level * .8f);
        defense = 10 + (int)(level * 1.2f);
        accuracy = .65f + level / 3500f;
        evasion = .02f + level / 1000f;

        combo = 3 + (int)Mathf.Pow(level, .25f);

        health = maxHealth;
    }
    protected override void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            SetStats();
            wildsManager.BossFight(gameObject);
        }
    }

    public override void AddReinforcement(EnemyInfo e)
    {
        reinforcements.Add(e);
    }
}