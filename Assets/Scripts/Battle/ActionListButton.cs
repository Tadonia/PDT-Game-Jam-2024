using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionListButton : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerClickHandler
{
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Button button;
    [SerializeField] Image iconImage;

    ActionList actionList;

    public void OnSelect(BaseEventData eventData)
    {
        actionList.ScrollTo(this);
        Debug.Log("Selected");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        actionList.OnButtonSubmit();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        actionList.OnButtonSubmit();
    }

    public void SetAction(string text, Sprite icon, ActionList list)
    {
        buttonText.text = text;
        actionList = list;
    }

    public float GetButtonHeight()
    {
        return button.GetComponent<RectTransform>().sizeDelta.y;
    }
}
