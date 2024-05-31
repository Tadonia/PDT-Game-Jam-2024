using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FireSpearMinigame : MonoBehaviour, IPlayerMinigame
{
    [SerializeField] RectTransform targetPoint;
    [SerializeField] RectTransform movingPoint;
    [SerializeField] float barLength = 1400f;
    [SerializeField] float speed = 700f;
    [SerializeField] float furthestDistance = 100f;
    [SerializeField] float damagePerStr = 5f;
    [SerializeField] AudioObject waterSound;

    PlayerInput playerInput;
    bool inputPressed;

    public void StartMinigame(PlayerCommander player, BattleActor[] targets)
    {
        playerInput = player.GetComponentInChildren<PlayerInput>();
        playerInput.currentActionMap.FindAction("Submit").performed += InputAction;
        StartCoroutine(Minigame(player, targets));
    }

    private void InputAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputPressed = true;
        }
    }

    IEnumerator Minigame(PlayerCommander player, BattleActor[] targets)
    {
        bool activated = false; // To prevent Submit accidentally being activated on first frame
        bool movingLeft = false;
        movingPoint.anchoredPosition = Vector2.left * barLength / 2f;

        // Wait until Submit (Enter key) is activated
        while (!activated || !inputPressed)
        {
            activated = true;
            movingPoint.anchoredPosition += speed * Time.deltaTime * (movingLeft ? Vector2.left : Vector2.right);

            // Flip when reaching the end of the bar
            if (Mathf.Abs(movingPoint.anchoredPosition.x) > barLength / 2f)
                movingLeft = !movingLeft;
            yield return null;
        }

        float dist = (targetPoint.position - movingPoint.position).magnitude;
        float maxDamage = player.actorStats.strength * damagePerStr;
        float damage = 0f;
        if (dist <= furthestDistance)
        {
            damage = maxDamage * (1f - dist / furthestDistance);    // Deal less damage the further away it is
            damage = ((int)(damage * 100f)) / 100f;                 // Convert to have only two decimal places
        }

        // Damage targets
        foreach (BattleActor target in targets)
        {
            target.DamageHealth(damage);
            BattleElementManager.Instance.AddDamageText(damage, target.transform.position + Vector3.up);
            waterSound.PlayAudio(target.transform.position);
        }

        yield return new WaitForSeconds(1);
        playerInput.currentActionMap.FindAction("Submit").performed -= InputAction;
        player.OnTurnEnd();
        Destroy(gameObject);
    }
}
