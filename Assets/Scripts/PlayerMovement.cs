using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed;

    CharacterController characterController;

    Vector3 moveDirection = Vector3.zero;
    private float moveSpeed;

    void Start()
    {
        // Get references
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float inputHori = Input.GetAxis("Horizontal");
        float inputVert = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(inputHori, 0f, inputVert).normalized;

        moveDirection += (inputDirection * moveSpeed * Time.deltaTime);
    }
}
