using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : BattleActor
{
    [SerializeField] Transform spriteTransform;
    [SerializeField] EnemyActionObject action;

    Vector3 startPos;

    protected override void Awake()
    {
        base.Awake();
        startPos = spriteTransform.position;
    }

    protected override void OnBattleStart()
    {
        base.OnBattleStart();

        // TODO: REMOVE
        SetStats(actorStats, actorStats.vitality * 5f + 25f, actorStats.spirit * 5f);
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        CameraPositionSwitcher.Instance.MoveToPosAndRot(new Vector3(0, 7.5f, -10f), Quaternion.Euler(22.5f, 0, 0), 0.25f);
        if (action == null)
        {
            StartCoroutine(Jump());
        }
        else
        {
            PerformAction();
        }
    }

    private void PerformAction()
    {
        PlayerCommander player = PlayerCommander.Instance;
        action.StartMinigame(this, new BattleActor[1] { player });
    }

    IEnumerator Jump()
    {
        spriteTransform.position = startPos;
        float startTime = Time.time;
        while (Time.time <= startTime + 1)
        {
            if (Time.time < startTime + 0.5f)
            {
                spriteTransform.position = Vector3.Lerp(startPos, startPos + Vector3.up, (Time.time - startTime) / 0.5f);
            }
            else
            {
                spriteTransform.position = Vector3.Lerp(startPos + Vector3.up, startPos, (Time.time - 0.5f - startTime) / 0.5f);
            }
            yield return null;
        }
        spriteTransform.position = startPos;
        OnTurnEnd();
    }
}
