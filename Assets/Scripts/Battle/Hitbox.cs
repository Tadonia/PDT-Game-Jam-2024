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
    private bool toggleHitbox;
    [SerializeField, Tooltip("Change how the hitbox behaves.")]
    AttackType attackType;
    [SerializeField, Tooltip("The allegiance of the hixbox.")]
    Allegiance allegiance;
    [SerializeField, Tooltip("World: Decides who goes first and player deals starting damage\nBattle: Deals damage during battle")] 
    int damage;

    [Header("Debug")]
    [SerializeField]
    bool drawHitbox;
    [SerializeField]
    Color hitboxColor = new Color(1, 0, 0, 0.3f);

    Collider[] colliders;

    void Start()
    {
        if (Application.IsPlaying(gameObject) && !FindColliders())
        {
            Debug.LogWarning(gameObject.name + " has no 3D colliders!");
            ToggleColliders();
        }
    }

    void OnEnable()
    {
        if (Application.IsPlaying(gameObject) && !FindColliders())
        {
            Debug.LogWarning(gameObject.name + " has no 3D colliders!");
            ToggleColliders();
        }
    }

    private bool FindColliders()
    {
        return (colliders = GetComponents<Collider>()) != null;
    }

    public void SetActive(bool set)
    {
        toggleHitbox = set;
        ToggleColliders();
        int count = 0;

        foreach (Collider c in colliders)
        {
            if (c.enabled == false) count++;
        }
    }

    /// <summary>
    /// Enable or disable all colliders of a hitbox based on active status.
    /// </summary>
    private void ToggleColliders()
    {
        FindColliders();

        foreach (Collider c in colliders)
        {
            c.enabled = toggleHitbox;
        }
    }

    public bool GetToggleStatus()
    {
        return toggleHitbox;
    }

    private void OnDrawGizmosSelected()
    {
        if (drawHitbox && toggleHitbox)
        {
            Gizmos.color = hitboxColor;
            Matrix4x4 rotationMatrix = transform.localToWorldMatrix;
            Gizmos.matrix = rotationMatrix;

            foreach (BoxCollider box in GetComponents<BoxCollider>())
            {
                if (box.enabled) Gizmos.DrawCube(box.center, box.size);
            }

            foreach (SphereCollider sphere in GetComponents<SphereCollider>())
            {
                if (sphere.enabled) Gizmos.DrawSphere(sphere.center, sphere.radius);
            }

            foreach (MeshCollider mesh in GetComponents<MeshCollider>())
            {
                if (mesh.enabled) Gizmos.DrawMesh(mesh.sharedMesh);
            }
        }
    }

    private void OnValidate()
    {
        SetActive(toggleHitbox);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (attackType == AttackType.World && collision.gameObject.TryGetComponent<IWorldHittable>(out IWorldHittable hittable))
        {
            hittable.OnWorldHit(allegiance, damage, gameObject);
        }

        if (attackType == AttackType.Battle)
        {

        }
    }

}
