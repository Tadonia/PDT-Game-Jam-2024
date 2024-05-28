using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCommander : BattleActor
{
    [Header("Player Commander Variables")]
    [SerializeField] ActionSelector actionSelector;
    [SerializeField] PlayerController playerController;
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerInput playerInput;

    public static PlayerCommander Instance { get; private set; }

    Dictionary<SkillCommandEnum, Action<BattleActor[]>> commandDictionary;
    Vector3 startPos;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        commandDictionary = new Dictionary<SkillCommandEnum, Action<BattleActor[]>>
        {
            { SkillCommandEnum.Ember, (BattleActor[] targets) => Ember(targets) },
            { SkillCommandEnum.FireSpear, (BattleActor[] targets) => FireSpear(targets) },
            { SkillCommandEnum.FlameBurst, (BattleActor[] targets) => FlameBurst(targets) },
            { SkillCommandEnum.HeatWave, (BattleActor[] targets) => HeatWave(targets) },
            { SkillCommandEnum.Torchlight, (BattleActor[] targets) => Torchlight(targets) },
        };
        playerController.enabled = false;
        startPos = transform.position;
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

    public void DoCommand(SkillCommandEnum skillCommand, ActionObject minigame, BattleActor[] targets)
    {
        actionSelector.gameObject.SetActive(false);
        minigame.StartMinigame(this, targets);
        //commandDictionary[skillCommand].Invoke(targets);
    }

    public void SetMovement(bool doMovement)
    {
        if (goBackCoroutine != null)
        {
            StopCoroutine(goBackCoroutine);
        }

        if (doMovement)
        {
            playerController.enabled = true;
            playerInput.SwitchCurrentActionMap("Player");
        }
        else
        {
            playerController.enabled = false;
            playerInput.SwitchCurrentActionMap("UI");
            goBackCoroutine = StartCoroutine(GoBack());
        }
    }

    Coroutine goBackCoroutine;
    IEnumerator GoBack()
    {
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        while (Vector3.Distance(transform.position, startPos) > 0.1f)
        {
            // TODO: GET RID OF MAGIC NUMBER 7
            characterController.Move((startPos - transform.position).normalized * 7.0f * Time.fixedDeltaTime);
            yield return waitForFixedUpdate;
        }
        goBackCoroutine = null;
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
