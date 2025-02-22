using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformCheckerStage6 : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private StageSixController stage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CollapseFloor"))
        {
            PlatformController questionController = other.gameObject.GetComponent<PlatformController>();

            if (questionController == null) return;

            if (questionController.questionIndex == stage.QuestionIndex)
                stage.QuestionIndex++;
        }

        if (other.gameObject.CompareTag("Death"))
        {
            userData.CurrentHealth -= 20;
        }

        if (other.gameObject.CompareTag("Teleporter"))
        {
            stage.WinPlayer();
        }
    }

}
