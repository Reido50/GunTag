using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController) characterController.enabled = false;
            other.transform.position = Vector3.zero;
            if (characterController) characterController.enabled = true;
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
