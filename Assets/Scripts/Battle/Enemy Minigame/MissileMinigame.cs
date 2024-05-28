using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileMinigame : MonoBehaviour, IEnemyMinigame
{
    [SerializeField] GameObject missile;
    [SerializeField] int numberOfMissiles;
    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;

    List<GameObject> missiles;

    public void StartMinigame(EnemyActor enemy, BattleActor[] targets)
    {
        (targets[0] as PlayerCommander).SetMovement(true);
        StartCoroutine(Minigame(enemy, targets));
    }

    public void RemoveMissile(GameObject missileObject)
    {
        missiles.Remove(missileObject);
    }

    IEnumerator Minigame(EnemyActor enemy, BattleActor[] targets)
    {
        Debug.Log(targets[0].gameObject.name);
        missiles = new List<GameObject>();
        for (int i = 0; i < numberOfMissiles; i++)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            GameObject projectile = Instantiate(missile, enemy.transform.position, Quaternion.identity);
            missiles.Add(projectile);
            projectile.GetComponent<MissileProjectile>().Initialise(this, targets[0] as PlayerCommander);
        }
        while (missiles.Count > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        (targets[0] as PlayerCommander).SetMovement(false);
        enemy.OnTurnEnd();
        Destroy(gameObject);
    }
}
