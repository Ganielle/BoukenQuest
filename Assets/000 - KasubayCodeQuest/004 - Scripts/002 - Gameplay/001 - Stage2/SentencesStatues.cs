using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentencesStatues : MonoBehaviour
{
    [SerializeField] private SentencesController controller;
    [SerializeField] private SentencesData data;

    public void SetSentenceData()
    {
        controller.SetupQuestionData(data);
    }
}
