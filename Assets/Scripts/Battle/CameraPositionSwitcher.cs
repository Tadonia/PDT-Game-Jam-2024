using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionSwitcher : MonoBehaviour
{
    public static CameraPositionSwitcher Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void MoveToPosAndRot(Vector3 pos, Quaternion rot, float moveTime)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(Transition(pos, rot, moveTime));
    }

    Coroutine transitionCoroutine;
    IEnumerator Transition(Vector3 pos, Quaternion rot, float moveTime)
    {
        float startTime = Time.time;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        while (Time.time < startTime + moveTime)
        {
            float t = (Time.time - startTime) / moveTime;
            transform.position = Vector3.Lerp(startPos, pos, t);
            transform.rotation = Quaternion.Lerp(startRot, rot, t);
            yield return null;
        }
        transform.position = pos;
        transform.rotation = rot;
        transitionCoroutine = null;
    }
}
