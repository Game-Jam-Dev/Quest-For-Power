using System.Collections.Generic;
using UnityEngine;

public abstract class WorldManager : MonoBehaviour {
    public virtual List<EnemyInfo> GetEnemies()
    {
        return null;
    }

    public virtual void WinBattle(){}
}