using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionList : MonoBehaviour
{
    [SerializeField] GameObject listButton;
    [SerializeField] RectTransform contextTransform;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] ActionSelector actionSelector;

    List<GameObject> listButtons = new List<GameObject>();

    public void CreateList(ActionObject[] actions)
    {
        ClearList();
        for (int i = 0; i < actions.Length; i++)
        {
            GameObject button = Instantiate(listButton, contextTransform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -120 * i, 0);
            ActionListButton actionListButton = button.GetComponentInChildren<ActionListButton>();
            actionListButton.SetAction(actions[i].actionName, actions[i].icon, this);
            listButtons.Add(button);
        }

        float height = actions.Length * 120;
        contextTransform.sizeDelta = new Vector2(contextTransform.sizeDelta.x, height);
        contextTransform.anchoredPosition = new Vector2(contextTransform.anchoredPosition.x, -height / 2f);
    }

    public void ClearList()
    {
        foreach (GameObject button in listButtons)
        {
            Destroy(button);
        }
        listButtons.Clear();
    }

    public void OnButtonSubmit()
    {
        actionSelector.OnButtonSubmit();
    }

    public void ScrollTo(ActionListButton actionListButton)
    {
        Canvas.ForceUpdateCanvases();
        Vector3 buttonPosition = scrollRect.transform.InverseTransformPoint(actionListButton.transform.position);
        float scrollHeight = scrollRect.GetComponent<RectTransform>().sizeDelta.y;
        float buttonHeight = actionListButton.GetButtonHeight();

        if (buttonPosition.y > scrollHeight / 2)
        {
            contextTransform.localPosition = new Vector3(contextTransform.localPosition.x, contextTransform.localPosition.y - buttonHeight);
        }

        if (buttonPosition.y < -scrollHeight / 2)
        {
            contextTransform.localPosition = new Vector3(contextTransform.localPosition.x, contextTransform.localPosition.y + buttonHeight);
        }
        //contextTransform.anchoredPosition = scrollRect.transform.InverseTransformPoint(contextTransform.position) - scrollRect.transform.InverseTransformPoint(actionListButton.transform.position);
    }
}