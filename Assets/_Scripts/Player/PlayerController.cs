using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Fields
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float speedChangeEasing;
    [Space(10)]
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpTimeout;
    [SerializeField] float jumpHeldGravity;
    [SerializeField] float gravity;

    [Header("Grounded Checking")]
    [SerializeField] LayerMask groundLayers;
    [SerializeField] float sphereRadius;
    [SerializeField] Vector3 sphereOffset;

    // Member Variables
    float _speed;
    float _verticalVelocity;
    bool _grounded;
    bool _jumping;
    bool _jumpTimedOut;

    // References
    CharacterController _Controller;
    PlayerInputHandler _Input;

    private void Awake()
    {
        _Controller = GetComponent<CharacterController>();
        _Input = GetComponent<PlayerInputHandler>();
    }


    private void OnEnable()
    {
        _Input.OnJump += Jump;
    }


    private void OnDisable()
    {
        _Input.OnJump -= Jump;
    }


    private void Update()
    {
        CheckGrounded();
        ApplyGravity();
        Move();
    }


    private void Move()
    {
        // Get Input
        Vector3 inputDirection = new Vector3(_Input.move.x, 0f, _Input.move.y).normalized;
        float inputMagnitude = _Input.move.magnitude;
        float targetSpeed = _Input.sprint ? sprintSpeed : moveSpeed;

        // Standing still
        if (_Input.move == Vector2.zero) targetSpeed = 0f;

        // Calc speed
        float currentSpeed = new Vector3(_Controller.velocity.x, 0f, _Controller.velocity.z).magnitude;
        _speed = Mathf.Lerp(currentSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeEasing);

        // Calc direction
        inputDirection = transform.right * _Input.move.x + transform.forward * _Input.move.y;

        // Apply Movement
        Vector3 deltaMove = inputDirection.normalized * (_speed * Time.deltaTime) + Vector3.up * _verticalVelocity * Time.deltaTime;
        _Controller.Move(deltaMove);
    }


    private void Jump()
    {
        if (_grounded && !_jumpTimedOut)
        {
            _jumping = true;

            // Apply jump velocity
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * jumpHeldGravity);

            StartCoroutine(JumpTimeOutCountdown());
        }
    }


    IEnumerator JumpTimeOutCountdown()
    {
        float currentJumpTimeOut = jumpTimeout;
        _jumpTimedOut = true;

        // Countdown
        while (currentJumpTimeOut > 0f)
        {
            currentJumpTimeOut -= Time.deltaTime;

            yield return null;
        }

        // Time Out over
        _jumpTimedOut = false;
    }


    private void CheckGrounded()
    {
        if (!_jumpTimedOut)
        {
            // Check for Ground Layers using a sphere
            Vector3 spherePos = transform.position + sphereOffset;
            Debug.DrawLine(spherePos, spherePos + Vector3.down * sphereRadius);
            _grounded = Physics.CheckSphere(spherePos, sphereRadius, groundLayers, QueryTriggerInteraction.Ignore);

            if (_jumping && _grounded)
            {
                _jumping = false;
            }
        }
    }


    private void ApplyGravity()
    {
        if (!_grounded)
        {
            if (_Input.jumpHeld && _jumping && _verticalVelocity > 0)
            {
                _verticalVelocity += jumpHeldGravity * Time.deltaTime;
            }
            else
            {
                _jumping = false;

                _verticalVelocity += gravity * Time.deltaTime;
            }
        }
    }
}
