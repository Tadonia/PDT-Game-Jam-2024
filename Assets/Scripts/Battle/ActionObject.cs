using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionObject", menuName = "ScriptableObjects/ActionObject")]
public class ActionObject : ScriptableObject
{
    public string actionName;
    public Sprite icon;
}
