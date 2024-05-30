using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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
    [SerializeField] RectTransform listCursor;
    [SerializeField] RectTransform enemyCursor;
    [SerializeField] RectTransform listWindow;
    [SerializeField] float listWindowRevealTime;
    [SerializeField] Vector3 listWindowTargetPostion;

    ActionList actionList;
    PlayerCommander playerCommander;
    bool isListRevealed;
    bool skillsSelected;
    bool itemsSelected;

    bool selectingEnemies;
    bool targetingAll;
    BattleActor[] enemyTargets;
    int selectedTarget;
    ActionObject currentMinigame;
    ActionListButton lastSelectedButton;
    List<RectTransform> cursorClones;

    private void Awake()
    {
        actionList = listWindow.GetComponent<ActionList>();
    }

    private void Start()
    {
        listCursor.gameObject.SetActive(false);
        enemyCursor.gameObject.SetActive(false);
        listWindow.gameObject.SetActive(false);
    }

    #region inputs
    public void NavigateInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnNavigateInput(context.ReadValue<Vector2>());
        }
    }

    public void SubmitInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSubmitInput();
        }
    }

    public void CancelInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnCancelInput();
        }
    }

    private void OnNavigateInput(Vector2 input)
    {
        if (selectingEnemies && !targetingAll)
        {
            selectedTarget += (int)input.y;
            if (selectedTarget > enemyTargets.Length - 1)
                selectedTarget = 0;
            else if (selectedTarget < 0)
                selectedTarget = enemyTargets.Length - 1;
            UIOverlayManager.Instance.SetUIElementPosition(enemyCursor, enemyTargets[selectedTarget].transform.position + new Vector3(0f, 2.5f, 0.33f));
        }
        else if (isListRevealed && input.x < 0)
        {
            HideList();
        }
    }

    private void OnSubmitInput()
    {
        if (selectingEnemies)
        {
            selectingEnemies = false;
            BattleActor[] targets = enemyTargets;
            if (!targetingAll) targets = new BattleActor[1] { enemyTargets[selectedTarget] };
            playerCommander.DoCommand(currentMinigame, targets);
        }
    }

    private void OnCancelInput()
    {
        if (selectingEnemies)
        {
            selectingEnemies = false;
            EventSystem.current.SetSelectedGameObject(lastSelectedButton.gameObject);
            lastSelectedButton.OnSelect(new BaseEventData(EventSystem.current));
            enemyCursor.gameObject.SetActive(false);
            currentMinigame = null;

            if (targetingAll)
            {
                targetingAll = false;
                for (int i = cursorClones.Count - 1; i >= 0; i--)
                {
                    Destroy(cursorClones[i].gameObject);
                }
                cursorClones.Clear();
            }
        }
    }
    #endregion

    public void OnTurnStart(PlayerCommander playerCommander)
    {
        this.playerCommander = playerCommander;
        isListRevealed = false;
        listWindow.anchoredPosition = Vector3.zero;
        enemyCursor.gameObject.SetActive(false);
        listWindow.gameObject.SetActive(false);

        BattleActor[] actors = FindObjectsOfType<BattleActor>();
        enemyTargets = (from a in actors orderby a.transform.position.y descending where a.allegiance is ActorAllegiance.Enemy select a).ToArray<BattleActor>();
        Debug.Log("Targets: " + enemyTargets.Length);
    }

    public void DoCommand(ActionObject minigame, ActionListButton selectedButton)
    {
        selectingEnemies = true;
        currentMinigame = minigame;
        lastSelectedButton = selectedButton;
        targetingAll = minigame.targetSelection == TargetSelection.Single ? false : true;

        EventSystem.current.SetSelectedGameObject(null);
        selectedTarget = 0;

        enemyCursor.gameObject.SetActive(true);
        UIOverlayManager.Instance.SetUIElementPosition(enemyCursor, enemyTargets[selectedTarget].transform.position + new Vector3(0f, 2.5f, 0.33f));

        if (targetingAll)
        {
            cursorClones = new List<RectTransform>();
            for (int i = 1; i < enemyTargets.Length; i++)
            {
                RectTransform cursorClone = Instantiate(enemyCursor.gameObject, UIOverlayManager.Instance.GetCanvas().transform).GetComponent<RectTransform>();
                UIOverlayManager.Instance.SetUIElementPosition(cursorClone, enemyTargets[i].transform.position + new Vector3(0f, 2.5f, 0.33f));
                cursorClones.Add(cursorClone);
            }
        }
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
        listWindow.gameObject.SetActive(true);
        if (moveListWindowCoroutine != null)
        {
            StopCoroutine(moveListWindowCoroutine);
            moveListWindowCoroutine = null;
        }
        moveListWindowCoroutine = StartCoroutine(MoveListWindow(listWindow.anchoredPosition, listWindowTargetPostion));
    }

    public void HideList()
    {
        if (skillsSelected)
        {
            EventSystem.current.SetSelectedGameObject(skillsButton.gameObject);
        }
        else if (itemsSelected)
        {
            EventSystem.current.SetSelectedGameObject(itemsButton.gameObject);
        }
        skillsSelected = false;
        itemsSelected = false;
        selectingEnemies = false;

        isListRevealed = false;
        enemyCursor.gameObject.SetActive(false);
        listWindow.gameObject.SetActive(false);
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
