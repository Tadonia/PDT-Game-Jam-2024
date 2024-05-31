using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] float delayBeforeBattleStarts = 1.0f;

    public static TurnManager Instance { get; private set; }

    public delegate void OnBattleStart();
    public OnBattleStart onBattleStart;

    public delegate void OnNextTurn(BattleActor actor);
    public OnNextTurn onNextTurn;

    List<BattleActor> battleActors;
    List<BattleActor> turnOrder;
    int turnCount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Start()
    {

    }

    public void OnBattleSceneStart()
    {
        StartCoroutine(StartBattleAfterDelay());
    }

    public void StartBattle()
    {
        battleActors = new List<BattleActor>(FindObjectsByType<BattleActor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        turnOrder = new List<BattleActor>(battleActors);
        turnOrder.Sort();
        turnOrder.Reverse();
        /*foreach (BattleActor battleActor in turnOrder)
        {
            Debug.Log(battleActor.name);
        }*/
        turnOrder[0].OnTurnStart();
        turnCount = 0;
        onBattleStart?.Invoke();
        onNextTurn?.Invoke(turnOrder[0]);
    }

    IEnumerator StartBattleAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeBattleStarts);
        StartBattle();
    }

    public void NextTurn()
    {
        turnCount++;
        int index = turnCount % turnOrder.Count;
        if (turnOrder[index] == null)
        {
            turnOrder.RemoveAt(index);
            if (index > turnOrder.Count - 1)
                index = 0;
        }
        turnOrder[index].OnTurnStart();
        onNextTurn?.Invoke(turnOrder[index]);
    }

    public void RemoveActor(BattleActor actor)
    {
        battleActors.Remove(actor);
        turnOrder.Remove(actor);
    }
}
