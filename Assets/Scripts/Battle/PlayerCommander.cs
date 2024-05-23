using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommander : BattleActor
{
    [Header("Player Commander Variables")]
    [SerializeField] ActionSelector actionSelector;

    public static PlayerCommander Instance { get; private set; }

    Dictionary<SkillCommandEnum, Action<BattleActor[]>> commandDictionary;

    protected override void Awake()
    {
        base.Awake();
        commandDictionary = new Dictionary<SkillCommandEnum, Action<BattleActor[]>>
        {
            { SkillCommandEnum.Ember, (BattleActor[] targets) => Ember(targets) },
            { SkillCommandEnum.FireSpear, (BattleActor[] targets) => FireSpear(targets) },
            { SkillCommandEnum.FlameBurst, (BattleActor[] targets) => FlameBurst(targets) },
            { SkillCommandEnum.HeatWave, (BattleActor[] targets) => HeatWave(targets) },
            { SkillCommandEnum.Torchlight, (BattleActor[] targets) => Torchlight(targets) },
        };
    }

    protected override void OnBattleStart()
    {
        base.OnBattleStart();

        // TODO: REMOVE
        SetStats(actorStats, actorStats.vitality * 5f + 25f, actorStats.spirit * 5f);
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        actionSelector.OnTurnStart(this);
        actionSelector.gameObject.SetActive(true);
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        actionSelector.gameObject.SetActive(false);
    }

    public void DoCommand(SkillCommandEnum skillCommand, GameObject minigame, BattleActor[] targets)
    {
        actionSelector.gameObject.SetActive(false);
        Instantiate(minigame).GetComponent<IPlayerMinigame>().StartMinigame(this, targets);
        //commandDictionary[skillCommand].Invoke(targets);
    }

    private void Ember(BattleActor[] targets)
    {
        Debug.Log("Ember");
        OnTurnEnd();
    }

    private void FireSpear(BattleActor[] targets)
    {
        Debug.Log("Fire Spear");
        OnTurnEnd();
    }

    private void FlameBurst(BattleActor[] targets)
    {
        Debug.Log("Flame Burst");
        OnTurnEnd();
    }

    private void HeatWave(BattleActor[] targets)
    {
        Debug.Log("Heat Wave");
        OnTurnEnd();
    }

    private void Torchlight(BattleActor[] targets)
    {
        Debug.Log("Torchlight");
        OnTurnEnd();
    }
}

public enum SkillCommandEnum
{
    None,
    Ember,
    FireSpear,
    FlameBurst,
    HeatWave,
    Torchlight
}
