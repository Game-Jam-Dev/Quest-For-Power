using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbCounter
{
    public EnemyBattle Enemy { get; private set; }

    public int Counter;

    public AbsorbCounter(EnemyBattle enemy, int counter)
    {
        Enemy = enemy;
        Counter = counter;
    }
}
