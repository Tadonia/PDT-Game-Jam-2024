using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleElementManager : MonoBehaviour
{
    [SerializeField] GameObject damageText;
    [SerializeField] int poolNumOfDamageTexts = 10;

    public static BattleElementManager Instance { get; private set; }

    Queue<GameObject> damageTexts;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        damageTexts = new Queue<GameObject>();
        for (int i = 0; i < poolNumOfDamageTexts; i++)
        {
            GameObject damageObject = Instantiate(damageText, UIOverlayManager.Instance.GetCanvas().transform);
            damageObject.SetActive(false);
            damageTexts.Enqueue(damageObject);
        }
    }

    public void AddDamageText(float damageValue, Vector3 position)
    {
        GameObject damageObject = damageTexts.Dequeue();
        damageObject.SetActive(true);
        damageObject.GetComponent<UIDamageText>().ShowText(damageValue, position);
        damageTexts.Enqueue(damageObject);
    }
}
