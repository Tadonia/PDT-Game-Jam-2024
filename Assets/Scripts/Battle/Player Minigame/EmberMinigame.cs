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
    bool activated = false;

    private void Update()
    {
        if (activated && EventSystem.current.currentInputModule.input.GetButtonDown("Submit"))
        {
            counter++;
            counterText.text = counter.ToString();
            ps.Emit(10);
        }
    }

    public void StartMinigame(PlayerCommander player, BattleActor[] targets)
    {
        activated = true;
        ps.transform.position = player.transform.position + Vector3.up;
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
        ParticleSystem.EmissionModule emission = ps.emission;
        emission.rateOverTime = 0;
        foreach (BattleActor target in targets)
        {
            ps.transform.position = target.transform.position + Vector3.up;
            ps.Emit(25);
            target.DamageHealth(damage);
        }
        yield return new WaitForSeconds(1);
        player.OnTurnEnd();
        Destroy(gameObject);
    }
}
