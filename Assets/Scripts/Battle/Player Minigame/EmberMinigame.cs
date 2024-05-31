using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EmberMinigame : MonoBehaviour, IPlayerMinigame
{
    [SerializeField] TMP_Text counterText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] ParticleSystem ps;
    [SerializeField] float minigameDuration;
    [SerializeField] AudioObject fireSound;
    [SerializeField] AudioObject chargeSound;

    int counter = 0;
    bool activated = false;
    PlayerInput playerInput;

    public void StartMinigame(PlayerCommander player, BattleActor[] targets)
    {
        playerInput = player.GetComponentInChildren<PlayerInput>();
        playerInput.currentActionMap.FindAction("Submit").performed += InputAction;
        activated = true;
        ps.transform.position = player.transform.position + Vector3.up;
        StartCoroutine(Minigame(player, targets));
    }

    private void InputAction(InputAction.CallbackContext context)
    {
        if (activated && context.performed)
        {
            counter++;
            counterText.text = counter.ToString();
            ps.Emit(5);
        }
    }

    IEnumerator Minigame(PlayerCommander player, BattleActor[] targets)
    {
        float timer = minigameDuration;
        AudioInstance chargeUp = chargeSound.PlayAudio(transform.position);
        while (timer > 0)
        {
            timerText.text = timer.ToString("F");
            timer = Mathf.Max(timer - Time.deltaTime, 0);
            yield return null;
        }
        float damage = player.actorStats.strength / 10f * counter;
        ParticleSystem.EmissionModule emission = ps.emission;
        emission.rateOverTime = 0;
        chargeUp.StopAudio();
        foreach (BattleActor target in targets)
        {
            ps.transform.position = target.transform.position + Vector3.up;
            ps.Emit(25);
            target.DamageHealth(damage);
            BattleElementManager.Instance.AddDamageText(damage, target.transform.position + Vector3.up);
            fireSound.PlayAudio(target.transform.position);
        }
        yield return new WaitForSeconds(1);
        playerInput.currentActionMap.FindAction("Submit").performed -= InputAction;
        player.OnTurnEnd();
        Destroy(gameObject);
    }
}
