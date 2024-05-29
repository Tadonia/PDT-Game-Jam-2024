using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIEvents : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public UnityEvent onAwake;
    public UnityEvent onStart;
    public UnityEvent onEnable;
    public UnityEvent onDisable;

    public UnityEvent onSelect;
    public UnityEvent onDeselect;
    public UnityEvent onSubmit;

    private void Awake()
    {
        onAwake?.Invoke();
    }

    private void Start()
    {
        onStart?.Invoke();
    }

    private void OnEnable()
    {
        onEnable?.Invoke();
    }

    private void OnDisable()
    {
        onDisable?.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelect?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onDeselect?.Invoke();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        onSubmit?.Invoke();
    }
}
