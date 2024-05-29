using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMinigame
{
    public void StartMinigame(PlayerCommander player, BattleActor[] targets);
}
