using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] float delayBeforeBattleStarts = 1.0f;

    public static TurnManager Instance { get; private set; }

    public delegate void OnBattleStart();
    public OnBattleStart onBattleStart;

    BattleActor[] battleActors;
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
        StartCoroutine(StartBattleAfterDelay());
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
        onBattleStart?.Invoke();
    }

    IEnumerator StartBattleAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeBattleStarts);
        StartBattle();
    }

    public void NextTurn()
    {
        turnCount++;
        turnOrder[turnCount % turnOrder.Count].OnTurnStart();
    }
}
