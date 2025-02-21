using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSixQuestionController : MonoBehaviour
{
    [SerializeField] private Collider answerACollider;
    [SerializeField] private Collider answerBCollider;
    [SerializeField] private PlatformController platformControllerA;
    [SerializeField] private PlatformController platformControllerB;

    public void InitializePlatform(bool ATurnOn, bool BTurnOn, string correctAnswer, string wrongAnswer, int index)
    {
        answerACollider.enabled = ATurnOn;
        answerBCollider.enabled = BTurnOn;

        platformControllerA.InitializeData(ATurnOn ? correctAnswer : wrongAnswer, index);
        platformControllerB.InitializeData(BTurnOn ? correctAnswer : wrongAnswer, index);
    }
}
