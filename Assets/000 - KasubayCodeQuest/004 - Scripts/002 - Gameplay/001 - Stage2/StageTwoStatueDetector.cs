using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageTwoStatueDetector : MonoBehaviour
{
    [SerializeField] private GamePlayerController controller;
    [SerializeField] private Button interactBtn;

    [Header("DEBUGGER")]
    [SerializeField] private GameObject statue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("QuestionQuest"))
        {
            interactBtn.interactable = true;
            statue = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("QuestionQuest"))
        {
            interactBtn.interactable = false;
            statue = null;
        }
    }

    private void Update()
    {
        if (controller.Interact && statue != null)
        {
            statue.GetComponent<SentencesStatues>().SetSentenceData();
            controller.InteractTurnOff();
        }
    }
}
