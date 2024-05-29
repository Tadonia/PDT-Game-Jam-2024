using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMinigame
{
    public void StartMinigame(EnemyActor enemy, BattleActor[] targets);
}
