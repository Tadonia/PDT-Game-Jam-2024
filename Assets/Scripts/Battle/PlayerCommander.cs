using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommander : BattleActor
{
    [SerializeField] ActionSelector actionSelector;

    public static PlayerCommander Instance { get; private set; }

    Dictionary<SkillCommandEnum, Action> commandDictionary;

    private void Awake()
    {
        commandDictionary = new Dictionary<SkillCommandEnum, Action>
        {
            { SkillCommandEnum.Ember, () => Ember() },
            { SkillCommandEnum.FireSpear, () => FireSpear() },
            { SkillCommandEnum.FlameBurst, () => FlameBurst() },
            { SkillCommandEnum.HeatWave, () => HeatWave() },
            { SkillCommandEnum.Torchlight, () => Torchlight() },
        };
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

    public void DoCommand(SkillCommandEnum skillCommand)
    {
        commandDictionary[skillCommand].Invoke();
    }

    private void Ember()
    {
        Debug.Log("Ember");
        OnTurnEnd();
    }

    private void FireSpear()
    {
        Debug.Log("Fire Spear");
        OnTurnEnd();
    }

    private void FlameBurst()
    {
        Debug.Log("Flame Burst");
        OnTurnEnd();
    }

    private void HeatWave()
    {
        Debug.Log("Heat Wave");
        OnTurnEnd();
    }

    private void Torchlight()
    {
        Debug.Log("Torchlight");
        OnTurnEnd();
    }
}

public enum SkillCommandEnum
{
    Ember,
    FireSpear,
    FlameBurst,
    HeatWave,
    Torchlight
}
