using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyActionObject", menuName = "ScriptableObjects/EnemyActionObject")]
public class EnemyActionObject : ScriptableObject
{
    public string actionName;
    public Sprite icon;
    public GameObject enemyMinigame;

    [HideInInspector] public GameObject minigameObject;
    IEnemyMinigame minigame;

    public void StartMinigame(EnemyActor enemy, BattleActor[] targets)
    {
        minigameObject = Instantiate(enemyMinigame);
        minigame = minigameObject.GetComponent<IEnemyMinigame>();
        minigame.StartMinigame(enemy, targets);
    }
}
