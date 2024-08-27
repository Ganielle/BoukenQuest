using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePuzzleMove : MonoBehaviour
{
    [SerializeField] private MovingPlatform rewardObject;
    [SerializeField] private float rewardResetDestination;
    [SerializeField] private float rewardDestination;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PuzzleBox"))
        {
            rewardObject.MoveToDestination(rewardDestination, true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("PuzzleBox"))
        {
            rewardObject.MoveToDestination(rewardResetDestination, false);
        }
    }
}
