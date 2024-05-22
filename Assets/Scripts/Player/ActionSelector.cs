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

    [Header("Others")]
    [SerializeField] RectTransform cursor;
    [SerializeField] RectTransform listWindow;
}
