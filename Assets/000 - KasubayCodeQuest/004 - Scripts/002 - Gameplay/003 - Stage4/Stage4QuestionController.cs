using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4QuestionController : MonoBehaviour
{
    [SerializeField] private Stage4SceneController controller;
    [SerializeField] private Stage4QuestionItem questionItem;
    [SerializeField] private List<Stage4AnswerItem> answerItems;
    [SerializeField] private int questionIndex;

    private void OnEnable()
    {
        CheckQuestions();
        controller.OnQuestionIndexChange += QuestionChange;
    }

    private void OnDisable()
    {
        controller.OnQuestionIndexChange -= QuestionChange;
    }

    private void QuestionChange(object sender, EventArgs e)
    {
        CheckQuestions();
    }

    private void CheckQuestions()
    {
        if (questionIndex != controller.QuestionIndex)
        {
            for (int a = 0; a < answerItems.Count; a++)
            {
                answerItems[a].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int a = 0; a < answerItems.Count; a++)
            {
                answerItems[a].gameObject.SetActive(true);
            }
        }
    }

    public void SetupAnswerItems(List<string> answer, string question, AudioClip questionClip, List<AudioClip> answerClips)
    {
        questionItem.question = question;
        questionItem.questionClip = questionClip;
        StartCoroutine(SetDatas(answer, answerClips));
    }

    IEnumerator SetDatas(List<string> answer, List<AudioClip> answerClips)
    {
        for (int a = 0; a < answer.Count; a++)
        {
            answerItems[a].answer = answer[a];
            answerItems[a].answerClip = answerClips[a];
            yield return null;
        }
    }
}
