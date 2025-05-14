using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSixQuestionController : MonoBehaviour
{
    [SerializeField] private Collider answerACollider;
    [SerializeField] private Collider answerBCollider;
    [SerializeField] private BoxCollider answerABoxCollision;
    [SerializeField] private BoxCollider answerBBoxCollision;
    [SerializeField] private PlatformController platformControllerA;
    [SerializeField] private PlatformController platformControllerB;

    [Header("DEBUGGER")]
    public int currentIndex;

    public void InitializePlatform(bool ATurnOn, bool BTurnOn, string correctAnswer, string wrongAnswer, int index, AudioClip clip)
    {
        answerACollider.enabled = ATurnOn;
        answerBCollider.enabled = BTurnOn;

        answerABoxCollision.enabled = ATurnOn;
        answerBBoxCollision.enabled = BTurnOn;

        platformControllerA.InitializeData(ATurnOn ? correctAnswer : wrongAnswer, index, clip);
        platformControllerB.InitializeData(BTurnOn ? correctAnswer : wrongAnswer, index, clip);
    }
}
