using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDamageText : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float lifetime = 1f;
    [SerializeField] float speed = 0.5f;

    UIOverlayManager manager;
    Vector3 startPos;
    Color startColor;

    private void Awake()
    {
        manager = UIOverlayManager.Instance;
        startColor = text.color;
    }

    public void ShowText(float value, Vector3 position)
    {
        text.text = value.ToString();
        text.color = startColor;
        startPos = position;
        if (animateTextCoroutine != null)
        {
            StopCoroutine(animateTextCoroutine);
        }
        animateTextCoroutine = StartCoroutine(AnimateText());
    }

    Coroutine animateTextCoroutine;
    IEnumerator AnimateText()
    {
        Vector3 targetPos = startPos + Vector3.up * speed * lifetime;
        float startTime = Time.time;
        while (Time.time < startTime + lifetime)
        {
            Vector3 position = Vector3.Lerp(startPos, targetPos, (Time.time - startTime) / lifetime);
            manager.SetUIElementPosition(transform, position);
            text.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), (Time.time - startTime) / lifetime);
            yield return null;
        }
        animateTextCoroutine = null;
        gameObject.SetActive(false);
    }
}
