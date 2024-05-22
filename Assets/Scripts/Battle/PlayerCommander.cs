using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommander : BattleActor
{
    [SerializeField] ActionSelector actionSelector;

    public static PlayerCommander Instance { get; private set; }

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
}

public enum SkillCommandEnum
{
    Ember,
    FireSpear,
    FlameBurst,
    HeatWave,
    Torchlight
}
