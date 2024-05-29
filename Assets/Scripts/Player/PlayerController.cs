using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IWorldHittable
{
    [SerializeField, Tooltip("Affects whether input can move the player or not. Player can still be moved by other means.")] 
    bool inputMovement = true;

    [Header("World Movement")]
    [SerializeField] float maxSpeed = 7.0f;
    [SerializeField] float acceleration = 21.0f;

    Vector2 moveInput;
    Vector3 velocity;

    CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        InputVelocity();
        MoveCharacter();
    }

    #region inputs
    public void MoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            moveInput = context.ReadValue<Vector2>();
        if (context.canceled)
            moveInput = Vector2.zero;
    }
    #endregion

    private void InputVelocity()
    {
        velocity.x = AccelerateValue(moveInput.x, velocity.x, maxSpeed);
        velocity.z = AccelerateValue(moveInput.y, velocity.z, maxSpeed);
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    private float AccelerateValue(float input, float speed, float maxSpeed)
    {
        float newSpeed = speed;
        // If there's input, try to accelerate
        if (Mathf.Abs(input) > 0.1f)
        {
            // Accelerate
            if (Mathf.Sign(input) == Mathf.Sign(newSpeed))
                newSpeed += input * acceleration * Time.fixedDeltaTime;
            // Decelerate if moving opposite of velocity
            else
            {
                float deceleration = input * acceleration * 2f * Time.fixedDeltaTime;
                float overAcceleration = newSpeed + deceleration;
                if (Mathf.Sign(overAcceleration) != Mathf.Sign(newSpeed))
                    newSpeed = overAcceleration;
                else
                    newSpeed += deceleration;
            }
            if (newSpeed > 0)
                newSpeed = Mathf.Min(newSpeed, Mathf.Sign(newSpeed) * maxSpeed);
            else if (newSpeed < 0)
                newSpeed = Mathf.Max(newSpeed, Mathf.Sign(newSpeed) * maxSpeed);
        }
        // For no inputs, decelerate to 0 speed
        else if (Mathf.Abs(speed) > 0f)
        {
            newSpeed -= Mathf.Sign(newSpeed) * acceleration * 2f * Time.fixedDeltaTime;
            if (Mathf.Sign(newSpeed) != Mathf.Sign(speed)) newSpeed = 0f;
        }

        // Decelerate if moving above maxSpeed
        if (Mathf.Abs(newSpeed) > maxSpeed)
        {
            newSpeed -= acceleration * 2f * Time.fixedDeltaTime;
            if (Mathf.Abs(newSpeed) < maxSpeed)
                newSpeed = maxSpeed * Mathf.Sign(newSpeed);
        }

        return newSpeed;
    }

    private void MoveCharacter()
    {
        Vector2 moveVelocity = new Vector2(velocity.x, velocity.z);
        if (moveVelocity.magnitude > maxSpeed)
        {
            moveVelocity = moveVelocity.normalized * maxSpeed;
        }
        cc.Move(new Vector3(moveVelocity.x, velocity.y, moveVelocity.y) * Time.fixedDeltaTime);
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void OnWorldHit()
    {
        Debug.Log("HiT!");
    }
}
