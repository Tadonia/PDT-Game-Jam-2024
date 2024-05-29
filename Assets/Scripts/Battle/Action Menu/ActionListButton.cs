using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionListButton : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerDownHandler
{
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Button button;
    [SerializeField] Image iconImage;

    ActionList actionList;
    ActionObject playerMinigame;

    public void OnSelect(BaseEventData eventData)
    {
        actionList.ScrollTo(this);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        actionList.DoCommand(playerMinigame, this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        actionList.DoCommand(playerMinigame, this);
    }

    public void SetAction(string text, Sprite icon, ActionList list, ActionObject minigame)
    {
        buttonText.text = text;
        actionList = list;
        playerMinigame = minigame;
    }

    public float GetButtonHeight()
    {
        return button.GetComponent<RectTransform>().sizeDelta.y;
    }
}
