using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Fields
    [Header("Controls")]
    [SerializeField] private GameObject cameraRoot;
    [SerializeField] private Vector2 mouseSensitivity = Vector2.one;
    [SerializeField] private bool invertY = false;

    [Header("Constraints")]
    [SerializeField] private Vector2 vertAngleContraints;

    // Member Variables
    float horiRotation = 0f;
    float vertRotation = 0f;

    // References
    PlayerInputHandler _Input;

    private void Awake()
    {
        _Input = GetComponent<PlayerInputHandler>();
    }


    private void Update()
    {
        Look();
    }


    private void Look()
    {
        // Get Input
        Vector2 deltaLookInput = _Input.look;

        // Calulate horizontal rotation
        float deltaHoriRotation = deltaLookInput.x * mouseSensitivity.x;
        horiRotation += deltaHoriRotation;

        // Calculate vertical rotation
        float deltaVertRotation = deltaLookInput.y * mouseSensitivity.y;
        deltaVertRotation *= invertY ? 1f : -1f;
        vertRotation += deltaVertRotation;
        vertRotation = Mathf.Clamp(vertRotation, vertAngleContraints.x, vertAngleContraints.y);

        // Apply rotations
        transform.rotation = Quaternion.Euler(Vector3.up * horiRotation);
        cameraRoot.transform.localRotation = Quaternion.Euler(Vector3.right * vertRotation);
    }
}
