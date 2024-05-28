using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionList : MonoBehaviour
{
    [SerializeField] GameObject listButton;
    [SerializeField] RectTransform contextTransform;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] ActionSelector actionSelector;

    List<Button> listButtons = new List<Button>();

    public void CreateList(ActionObject[] actions)
    {
        ClearList();
        for (int i = 0; i < actions.Length; i++)
        {
            GameObject buttonObject = Instantiate(listButton, contextTransform);
            buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -120 * i, 0);
            Button button = buttonObject.GetComponentInChildren<Button>();

            if (i != 0)
            {
                SetNavigationUp(button, listButtons[i - 1]);
                SetNavigationDown(listButtons[i - 1], button);
            }
            if (i == actions.Length - 1)
            {
                SetNavigationDown(button, listButtons[0]);
            }

            ActionListButton actionListButton = buttonObject.GetComponentInChildren<ActionListButton>();
            actionListButton.SetAction(actions[i].actionName, actions[i].icon, this, actions[i].command, actions[i]);
            listButtons.Add(button);
        }

        float height = actions.Length * 120;
        contextTransform.sizeDelta = new Vector2(contextTransform.sizeDelta.x, height);
        contextTransform.anchoredPosition = new Vector2(contextTransform.anchoredPosition.x, -height / 2f);
        EventSystem.current.SetSelectedGameObject(listButtons[0].gameObject);
    }

    private void SetNavigationUp(Button button1, Button button2)
    {
        Navigation newNavigation = button1.navigation;
        newNavigation.selectOnUp = button2;
        button1.navigation = newNavigation;
    }

    private void SetNavigationDown(Button button1, Button button2)
    {
        Navigation newNavigation = button1.navigation;
        newNavigation.selectOnDown = button2;
        button1.navigation = newNavigation;
    }

    public void ClearList()
    {
        foreach (Button button in listButtons)
        {
            Destroy(button.gameObject);
        }
        listButtons.Clear();
    }

    public void DoCommand(SkillCommandEnum command, ActionObject minigame, ActionListButton selectedButton)
    {
        actionSelector.DoCommand(command, minigame, selectedButton);
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