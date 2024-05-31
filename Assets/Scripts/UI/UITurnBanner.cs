using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITurnBanner : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] TMP_Text turnText;
    [SerializeField] AudioObject turnSound;

    public void Start()
    {
        TurnManager.Instance.onNextTurn += SetTurn;
        TurnManager.Instance.onBattleStart += OnBattleStart;
        background.enabled = false;
        turnText.enabled = false;
    }

    private void OnDisable()
    {
        TurnManager.Instance.onNextTurn -= SetTurn;
        TurnManager.Instance.onBattleStart -= OnBattleStart;
    }

    public void OnBattleStart()
    {
        background.enabled = true;
        turnText.enabled = true;
    }

    public void SetTurn(BattleActor actor)
    {
        turnText.text = string.Format("{0}'s Turn", actor.name);
        background.color = actor.allegiance switch
        {
            ActorAllegiance.Player => new Color(0f, 0f, 0.75f, 0.5f),
            ActorAllegiance.Enemy => new Color(0.75f, 0f, 0f, 0.5f),
            _ => new Color(0f, 0f, 0f, 0.5f),
        };
        turnSound.PlayAudio(Vector3.zero);
    }
}
