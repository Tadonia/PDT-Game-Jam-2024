using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class BattleActor : MonoBehaviour, IComparable<BattleActor>
{
    [Header("Base BattleActor Variables")]
    public ActorStats actorStats;
    public bool debugMessages = true;

    public virtual void OnTurnStart()
    {
        if (debugMessages) Debug.Log(name + "'s Turn Started");
    }

    public virtual void OnTurnEnd()
    {
        if (debugMessages) Debug.Log(name + "'s Turn Ended");
        TurnManager.Instance.NextTurn();
    }

    public int CompareTo(BattleActor other)
    {
        if (other == null) return 0;
        return actorStats.CompareTo(other.actorStats);
    }
}

[System.Serializable]
public class ActorStats : IComparable<ActorStats>
{
    public float strength = 5f;
    public float speed = 5f;
    public float defence = 5f;
    public float resistance = 5f;

    public ActorStats(float strength, float speed, float defence, float resistance)
    {
        this.strength = strength;
        this.speed = speed;
        this.defence = defence;
        this.resistance = resistance;
    }

    public int CompareTo(ActorStats other)
    {
        if (other == null) return 0;
        return speed.CompareTo(other.speed);
    }
}