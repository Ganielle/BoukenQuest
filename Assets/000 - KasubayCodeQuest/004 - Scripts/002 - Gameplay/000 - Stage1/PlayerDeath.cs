using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private CharacterController characterController;

    [Header("DEBUGGER")]
    [SerializeField] private float temporaryHealth;
    [SerializeField] private Vector3 checkpointPos;

    private void OnEnable()
    {
        temporaryHealth = userData.CurrentHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            userData.CurrentHealth -= 20;
            characterController.Move(Vector3.zero);
            transform.position = new Vector3(checkpointPos.x, checkpointPos.y, transform.position.z);
        }

        else if (other.CompareTag("Checkpoint"))
        {
            checkpointPos = other.transform.position;
            other.gameObject.SetActive(false);
        }
    }
}
