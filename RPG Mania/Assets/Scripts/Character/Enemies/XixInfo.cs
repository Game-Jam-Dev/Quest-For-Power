using UnityEngine;
using System.Collections.Generic;

public class XixInfo : EnemyInfo {
    private List<EnemyInfo> reinforcements = new List<EnemyInfo>();

    protected override void Start() {
        base.Start();

        ea.AssignElement(element);
    }
    protected override void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            wildsManager.BossFight(gameObject);
        }
    }

    public override void AddReinforcement(EnemyInfo e)
    {
        reinforcements.Add(e);
    }
}