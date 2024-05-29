using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AttackType
{
    World,
    Battle
}

[ExecuteAlways]
[ExecuteInEditMode]
public class Hitbox : MonoBehaviour
{
    [SerializeField]
    bool active;
    [SerializeField, Tooltip("Change how the hitbox behaves.")]
    AttackType attackType;
    [SerializeField, Tooltip("World: Decides who goes first and player deals starting damage\nBattle: Deals damage during battle")] 
    int damage;

    [Header("Debug")]
    [SerializeField]
    bool drawHitbox;
    [SerializeField, Range(0.0f, 1.0f)]
    float hitboxOpacity = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.IsPlaying(gameObject) && !CheckColliders())
        {
            Debug.LogWarning(gameObject.name + " has no 3D colliders!");
            ToggleColliders();
        }
    }

    private bool CheckColliders()
    {
        return GetComponents<Collider>() != null;
    }

    public void SetActive(bool set)
    {
        active = set;
        ToggleColliders();
        int count = 0;

        foreach (Collider c in GetComponents<Collider>())
        {
            if (c.enabled == false) count++;
        }
    }

    /// <summary>
    /// Enable or disable all colliders of a hitbox based on active status.
    /// </summary>
    private void ToggleColliders()
    {
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = active;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (drawHitbox && active)
        {
            Gizmos.color = new(1, 0, 0, hitboxOpacity);

            foreach (BoxCollider box in GetComponents<BoxCollider>())
            {
                if (box.enabled) Gizmos.DrawCube(box.center + transform.position, box.size);
            }

            foreach (SphereCollider sphere in GetComponents<SphereCollider>())
            {
                if (sphere.enabled) Gizmos.DrawSphere(sphere.center + transform.position, sphere.radius);
            }

            foreach (MeshCollider mesh in GetComponents<MeshCollider>())
            {
                if (mesh.enabled) Gizmos.DrawMesh(mesh.sharedMesh, transform.position);
            }
        }
    }

    private void OnValidate()
    {
        SetActive(active);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (attackType == AttackType.World && collision.gameObject.TryGetComponent<IWorldHittable>(out IWorldHittable hittable))
        {
            hittable.OnWorldHit();
        }

        if (attackType == AttackType.Battle)
        {

        }
    }
}
