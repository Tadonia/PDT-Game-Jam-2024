using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : BattleActor
{
    [SerializeField] Transform spriteTransform;

    Vector3 startPos;

    public void Awake()
    {
        startPos = spriteTransform.position;
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        StartCoroutine(Jump());
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
