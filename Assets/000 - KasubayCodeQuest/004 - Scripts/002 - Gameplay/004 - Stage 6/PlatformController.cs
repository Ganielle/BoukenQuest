using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private StageSixController controller;
    [SerializeField] private TextMeshPro answerTMP;

    [Header("DEBUGGER")]
    public int questionIndex;
    public AudioClip clip;

    private void OnEnable()
    {
        controller.OnQuestionIndexChange += QuestionIndexChange;
    }

    private void QuestionIndexChange(object sender, EventArgs e)
    {
        CheckQuestionIndex();
    }

    public void InitializeData(string answer, int index, AudioClip clip)
    {
        answerTMP.text = answer;
        questionIndex = index;
        this.clip = clip;
        CheckQuestionIndex();
    }

    private void CheckQuestionIndex()
    {
        if (questionIndex == controller.QuestionIndex)
            answerTMP.gameObject.SetActive(true);
        else
            answerTMP.gameObject.SetActive(false);
    }
}
