using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct EnemyAppearances
{
    public GameObject enemy;
    public int enemySpawnWeight;
}

public class WorldEnemyController : MonoBehaviour
{
    [SerializeField] NavMeshAgent enemyAi;
    [SerializeField] SpriteRenderer sprite;

    [Header("World Variables")]
    [SerializeField] float detectionRange;
    [SerializeField] float chaseRange;
    [SerializeField, Range(1, 60)] int targetUpdateRate;

    [Header("Battle Variables")]
    [SerializeField, Range(1,5)] int minEnemies;
    [SerializeField, Range(1,5)] int maxEnemies;
    [SerializeField, Tooltip("The selection of enemies to possibly spawn when battle starts. The first enemy is guaranteed, so it's weight is not counted.")] 
    EnemyAppearances[] enemyPool;
    [Space]

    //Make private when we have proper way of getting player
    [SerializeField] Transform followTarget;
    int numberOfEnemies;
    GameObject[] enemiesToSpawn;
    bool isAttacked;
    bool isChasing;
    Vector3 homePos;
    WaitForSeconds targetUpdateWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        homePos = transform.position;
        targetUpdateWaitTime = new WaitForSeconds(1.0f / targetUpdateRate);
        StartCoroutine(ChaseRate());
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite)
        {
            sprite.transform.localRotation = Quaternion.Euler(15.0f, -transform.rotation.eulerAngles.y, 0.0f);
        }

        CheckInRange();
    }

    void CheckInRange()
    {
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(followTarget.position.x, followTarget.position.z);
        float distance = Vector2.Distance(enemyPos, targetPos);

        if (distance <= detectionRange)
        {
            isChasing = true;
            ChasePlayer();
        }
        else if (isChasing && distance >= chaseRange)
        {
            isChasing = false;
            enemyAi.destination = homePos;
        }
    }

    void ChasePlayer()
    {
        if (isChasing)
        {
            enemyAi.destination = followTarget.position;
        }
    }

    IEnumerator ChaseRate()
    {
        ChasePlayer();
        yield return targetUpdateWaitTime;
        StartCoroutine(ChaseRate());
    }

    void StartBattle()
    {
        int totalWeight = 0;
        numberOfEnemies = Random.Range(minEnemies, maxEnemies + 1);
        enemiesToSpawn = new GameObject[numberOfEnemies];
        enemiesToSpawn[0] = enemyPool[0].enemy;

        // First enemy in list is not counted as they are guaranteed to appear
        for (int i = 1; i < enemyPool.Length; i++)
        {
            totalWeight += enemyPool[i].enemySpawnWeight;
        }

        for (int i = 1; i < numberOfEnemies; i++)
        {
            int randomChoice = Random.Range(0, totalWeight);
            enemiesToSpawn[i] = WeightedSelection(randomChoice);
        }

        //!!! Have some function to send enemiesToSpawn and isAttacked to the battle scene !!!
    }

    GameObject WeightedSelection(int number)
    {
        for (int i = 1; i < enemyPool.Length; i++)
        {
            number -= enemyPool[i].enemySpawnWeight;

            if (number <= 0)
            {
                return enemyPool[i].enemy;
            }
        }
        return enemyPool[1].enemy;
    }
}
