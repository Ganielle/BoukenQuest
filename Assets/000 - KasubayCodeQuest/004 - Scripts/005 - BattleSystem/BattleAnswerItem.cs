using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleAnswerItem : MonoBehaviour
{
    public string Answer
    {
        get => answer;
    }

    //  =======================

    [SerializeField] private BattleSystemController battleSystem;
    [SerializeField] private TextMeshProUGUI answerTMP;

    [Header("DEBUGGER")]
    [SerializeField] private string answer;

    public void SetupData(string answer)
    {
        this.answer = answer;
        answerTMP.text = answer;
    }

    public void SelectAnswer()
    {
        battleSystem.CurrentPlayerAnswer = this;
    }
}
