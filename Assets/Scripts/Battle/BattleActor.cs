using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleActor : MonoBehaviour, IComparable<BattleActor>
{
    [Header("Base Battle Actor Variables")]
    public ActorStats actorStats;
    public ActorAllegiance allegiance;
    public GameObject statsBarPrefab;
    public Vector3 statsBarOffset = new Vector3(0f, 2.17f, 0.33f);
    public bool debugMessages = true;

    [Header("Audio Objects")]
    public AudioObject attackSound;
    public AudioObject damageTakenSound;

    protected float maxHP;
    protected float currentHP;
    protected float maxMP;
    protected float currentMP;

    protected UIStatsBar statsBar;
    protected bool battleStarted;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        TurnManager.Instance.onBattleStart += OnBattleStart;
    }

    protected virtual void OnDisable()
    {
        TurnManager.Instance.onBattleStart -= OnBattleStart;
    }

    protected virtual void Update()
    {
        if (battleStarted)
        {
            if (statsBar)
                UIOverlayManager.Instance.SetUIElementPosition(statsBar.transform, transform.position + statsBarOffset);
        }
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
        if (statsBar) Destroy(statsBar.gameObject);
    }

    protected virtual void OnBattleStart()
    {
        statsBar = Instantiate(statsBarPrefab, UIOverlayManager.Instance.GetCanvas().transform).GetComponent<UIStatsBar>();
        battleStarted = true;
    }

    public virtual void OnTurnStart()
    {
        if (debugMessages) Debug.Log(name + "'s Turn Started");
    }

    public virtual void OnTurnEnd()
    {
        if (debugMessages) Debug.Log(name + "'s Turn Ended");
        TurnManager.Instance.NextTurn();
    }

    public virtual void SetStats(ActorStats actorStats, float currentHP, float currentMP)
    {
        this.actorStats = actorStats;
        maxHP = actorStats.vitality * 5f + 25f;
        maxMP = actorStats.spirit * 5f;
        this.currentHP = currentHP;
        this.currentMP = currentMP;

        statsBar.UpdateStatsBar(currentHP, currentMP, maxHP, maxMP);
    }

    public virtual void DamageHealth(float damage)
    {
        currentHP = Mathf.Max(currentHP - damage, 0);
        statsBar.UpdateStatsBar(currentHP, currentMP, maxHP, maxMP);
        if (damage > 0 && damageTakenSound)
            damageTakenSound.PlayAudio(transform.position);
        if (currentHP <= 0)
        {
            OnDeath();
        }
    }

    public virtual void HealHealth(float heal)
    {
        currentHP = Mathf.Min(currentHP + heal, maxMP);
        statsBar.UpdateStatsBar(currentHP, currentMP, maxHP, maxMP);
    }

    public virtual void ReduceMP(float MPAmount)
    {
        currentMP -= MPAmount;
        statsBar.UpdateStatsBar(currentHP, currentMP, maxHP, maxMP);
    }

    public virtual void RecoverMP(float MPAmount)
    {
        currentMP += MPAmount;
        statsBar.UpdateStatsBar(currentHP, currentMP, maxHP, maxMP);
    }

    protected virtual void OnDeath()
    {
        TurnManager.Instance.RemoveActor(this);
        Destroy(gameObject);
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
    public float vitality = 5f;
    public float strength = 5f;
    public float spirit = 5f;
    public float speed = 5f;
    public float defence = 5f;
    public float resistance = 5f;

    public ActorStats(float vitality, float strength, float spirit, float speed, float defence, float resistance)
    {
        this.vitality = vitality;
        this.strength = strength;
        this.spirit = spirit;
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

public enum ActorAllegiance
{
    None,
    Player,
    Enemy
}