using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    BattleActor[] battleActors;
    List<BattleActor> turnOrder;
    int turnCount;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    public void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        battleActors = FindObjectsByType<BattleActor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        turnOrder = new List<BattleActor>(battleActors);
        turnOrder.Sort();
        turnOrder.Reverse();
        /*foreach (BattleActor battleActor in turnOrder)
        {
            Debug.Log(battleActor.name);
        }*/
        turnOrder[0].OnTurnStart();
        turnCount = 0;
    }

    public void NextTurn()
    {
        turnCount++;
        turnOrder[turnCount % turnOrder.Count].OnTurnStart();
    }
}
