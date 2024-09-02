using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SentencesController : MonoBehaviour
{
    [SerializeField] private UserData userData;

    [Space]
    [SerializeField] private List<SentenceArrangeItem> answerItems;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private TextMeshProUGUI objectives;

    [Header("DEBUGGER")]
    [SerializeField] private SentencesData currentData;
    [SerializeField] private int currentObjectives;

    public void SetupQuestionData(SentencesData data)
    {
        currentData = data;

        for (int a = 0; a < answerItems.Count; a++)
        {
            if (a < data.SentencesList.Count)
                answerItems[a].SetData(data.SentencesList[a].characters, data.SentencesList[a].translation);
            else
                answerItems[a].SetData("", "");
        }

        questionPanel.SetActive(true); 
        gameplayPanel.SetActive(false);
    }

    public void CheckAnswer()
    {
        bool canDamage = false;

        for (int a = 0; a < answerItems.Count; a++)
        {
            if (answerItems[a].currentIndex.ToString() != currentData.SentencesList[a].index)
            {
                canDamage = true;
                break;
            }
        }

        if (canDamage)
            userData.CurrentHealth -= 20;

        if (currentObjectives >= 10)
        {

        }
        else
        {
            objectives.text = $"Statues Answered:\n {currentObjectives} / 10";
            currentObjectives++;
        }
    }
}
