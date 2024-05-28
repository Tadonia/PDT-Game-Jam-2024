using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : MonoBehaviour
{
    [SerializeField] GameObject targetCircle;
    [SerializeField] float horizontalSpeed = 5f;
    [SerializeField] float gravity = -10f;
    [SerializeField] LayerMask playerMask;

    MissileMinigame minigame;
    PlayerCommander player;
    Transform playerTransform;
    GameObject circleObject;
    bool initialised = false;
    Vector3 velocity;
    float timer;
    float timeTaken;

    public void Initialise(MissileMinigame minigame, PlayerCommander player)
    {
        initialised = true;
        this.minigame = minigame;
        this.player = player;
        playerTransform = player.transform;
        circleObject = Instantiate(targetCircle, playerTransform.position, Quaternion.identity);

        Vector3 playerHoriPos = playerTransform.position;
        playerHoriPos.y = 0;
        Vector3 missileHoriPos = transform.position;
        missileHoriPos.y = 0;

        Vector3 horiVector = (playerHoriPos - missileHoriPos);
        float horiDist = horiVector.magnitude;
        float t = horiDist / horizontalSpeed;

        // v = u + at
        // s = ut + (1/2)at^2
        // v^2 = u^2 + 2as

        float yu = -gravity * t / 2;
        velocity = horiVector.normalized * horizontalSpeed;
        velocity.y = yu;
        transform.rotation = Quaternion.LookRotation(velocity);
        timeTaken = t;
    }

    private void FixedUpdate()
    {
        if (!initialised)
            return;
        velocity.y += gravity * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
        transform.rotation = Quaternion.LookRotation(velocity);
        if (Vector3.Distance(transform.position, circleObject.transform.position) < 0.1f || timer >= timeTaken)
        {
            Collider[] colliders = Physics.OverlapSphere(circleObject.transform.position, 1.0f, playerMask, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
                player.DamageHealth(5f);
            minigame.RemoveMissile(gameObject);
            Destroy(gameObject);
        }
        timer += Time.fixedDeltaTime;
    }

    private void OnDestroy()
    {
        Destroy(circleObject);
    }
}
