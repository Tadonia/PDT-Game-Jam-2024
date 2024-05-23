using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSelector : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button attackButton;
    [SerializeField] Button skillsButton;
    [SerializeField] Button itemsButton;
    [SerializeField] Button runButton;

    [Header("Action Lists")]
    [SerializeField] ActionObject[] skillActions;
    [SerializeField] ActionObject[] itemActions;

    [Header("Others")]
    [SerializeField] RectTransform cursor;
    [SerializeField] RectTransform listWindow;
    [SerializeField] float listWindowRevealTime;
    [SerializeField] Vector3 listWindowTargetPostion;

    ActionList actionList;
    PlayerCommander playerCommander;
    bool isListRevealed;
    bool skillsSelected;
    bool itemsSelected;

    private void Awake()
    {
        actionList = listWindow.GetComponent<ActionList>();
    }

    public void OnTurnStart(PlayerCommander playerCommander)
    {
        this.playerCommander = playerCommander;
        isListRevealed = false;
        listWindow.anchoredPosition = Vector3.zero;
    }

    public void DoCommand(SkillCommandEnum command)
    {
        playerCommander.DoCommand(command);
    }

    public void AttackButton()
    {
        playerCommander.OnTurnEnd();
    }

    public void SkillsButton()
    {
        if (!isListRevealed || itemsSelected)
        {
            skillsSelected = true;
            itemsSelected = false;
            actionList.CreateList(skillActions);
            RevealList();
        }
        else
            HideList();
    }

    public void ItemsButton()
    {
        if (!isListRevealed || skillsSelected)
        {
            itemsSelected = true;
            skillsSelected = false;
            actionList.CreateList(itemActions);
            RevealList();
        }
        else
            HideList();
    }

    public void RunButton()
    {
        playerCommander.OnTurnEnd();
    }

    public void RevealList()
    {
        if (isListRevealed) return;
        isListRevealed = true;
        if (moveListWindowCoroutine != null)
        {
            StopCoroutine(moveListWindowCoroutine);
            moveListWindowCoroutine = null;
        }
        moveListWindowCoroutine = StartCoroutine(MoveListWindow(listWindow.anchoredPosition, listWindowTargetPostion));
    }

    public void HideList()
    {
        isListRevealed = false;
        if (moveListWindowCoroutine != null)
        {
            StopCoroutine(moveListWindowCoroutine);
            moveListWindowCoroutine = null;
        }
        moveListWindowCoroutine = StartCoroutine(MoveListWindow(listWindow.anchoredPosition, Vector3.zero));
    }

    Coroutine moveListWindowCoroutine;
    IEnumerator MoveListWindow(Vector3 initialPosition, Vector3 targetPosition)
    {
        float timer = 0f;
        while (timer <= listWindowRevealTime)
        {
            listWindow.anchoredPosition = Vector3.Lerp(initialPosition, targetPosition, timer / listWindowRevealTime);
            timer += Time.deltaTime;
            yield return null;
        }
        listWindow.anchoredPosition = targetPosition;
    }
}
