using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Stage3AnswerItem : MonoBehaviour
{
    [SerializeField] private Stage3Controller controller;
    [SerializeField] private TextMeshProUGUI answerTMP;
    [SerializeField] private GameObject selectedObj;

    [Header("DEBUGGER")]
    [SerializeField] public string answer;

    private void OnEnable()
    {
        answerTMP.text = answer;
        CheckAnswerSelected();
        controller.OnAnswerItemChange += AnswerChange;
    }

    private void OnDisable()
    {
        controller.OnAnswerItemChange -= AnswerChange;
    }

    private void AnswerChange(object sender, EventArgs e)
    {
        CheckAnswerSelected();
    }

    private void CheckAnswerSelected()
    {
        if (controller.SelectedAnswer == this)
            selectedObj.SetActive(true);
        else
            selectedObj.SetActive(false);
    }

    public void SelectAnswer()
    {
        if (controller.SelectedAnswer == this) return;

        controller.SelectedAnswer = this;
    }
}
