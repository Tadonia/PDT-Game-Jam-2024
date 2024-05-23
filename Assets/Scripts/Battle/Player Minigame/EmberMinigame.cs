using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EmberMinigame : MonoBehaviour, IPlayerMinigame
{
    [SerializeField] TMP_Text counterText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] ParticleSystem ps;

    int counter = 0;

    private void Update()
    {
        if (EventSystem.current.currentInputModule.input.GetButtonDown("Submit"))
        {
            counter++;
            counterText.text = counter.ToString();
            ps.Emit(10);
        }
    }

    public void StartMinigame(PlayerCommander player, BattleActor[] targets)
    {
        StartCoroutine(Minigame(player, targets));
    }

    IEnumerator Minigame(PlayerCommander player, BattleActor[] targets)
    {
        float timer = 5f;
        while (timer > 0)
        {
            timerText.text = timer.ToString("F");
            timer = Mathf.Max(timer - Time.deltaTime, 0);
            yield return null;
        }
        float damage = player.actorStats.strength / 10f * counter;
        foreach (BattleActor target in targets)
        {
            target.DamageHealth(damage);
        }
        player.OnTurnEnd();
        Destroy(gameObject);
    }
}
