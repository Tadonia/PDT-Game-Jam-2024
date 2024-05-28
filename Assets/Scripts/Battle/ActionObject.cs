using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionObject", menuName = "ScriptableObjects/ActionObject")]
public class ActionObject : ScriptableObject
{
    public string actionName;
    public Sprite icon;
    public SkillCommandEnum command;
    public GameObject attackMinigame;

    [HideInInspector] public GameObject minigameObject;
    IPlayerMinigame minigame;

    public void StartMinigame(PlayerCommander player, BattleActor[] targets)
    {
        minigameObject = Instantiate(attackMinigame);
        minigame = minigameObject.GetComponent<IPlayerMinigame>();
        minigame.StartMinigame(player, targets);
    }
}
