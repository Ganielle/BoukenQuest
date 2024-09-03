using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentencesStatues : MonoBehaviour
{
    [SerializeField] private SentencesController controller;
    [SerializeField] private SentencesData data;
    [SerializeField] private GameObject questIndicator;

    public void SetSentenceData()
    {
        controller.SetupQuestionData(data);
        questIndicator.SetActive(false);
        gameObject.SetActive(false);
    }
}
