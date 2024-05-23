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
    SkillCommandEnum skillCommand;
    GameObject playerMinigame;

    public void OnSelect(BaseEventData eventData)
    {
        actionList.ScrollTo(this);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        actionList.DoCommand(skillCommand, playerMinigame, this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        actionList.DoCommand(skillCommand, playerMinigame, this);
    }

    public void SetAction(string text, Sprite icon, ActionList list, SkillCommandEnum command, GameObject minigame)
    {
        buttonText.text = text;
        actionList = list;
        skillCommand = command;
        playerMinigame = minigame;
    }

    public float GetButtonHeight()
    {
        return button.GetComponent<RectTransform>().sizeDelta.y;
    }
}
