using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Stage4SceneController : MonoBehaviour
{
    private event EventHandler QuestionIndexChange;
    public event EventHandler OnQuestionIndexChange
    {
        add
        {
            if (QuestionIndexChange == null || !QuestionIndexChange.GetInvocationList().Contains(value))
                QuestionIndexChange += value;
        }
        remove { QuestionIndexChange -= value; }
    }
    public int QuestionIndex
    {
        get => questionIndex;
        set
        {
            questionIndex = value;
            QuestionIndexChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  =======================

    [SerializeField] private SchoolSceneData schoolSceneData;
    [SerializeField] private UserData userData;
    [SerializeField] private List<Stage4QuestionData> questionDatas;
    [SerializeField] private List<Stage4QuestionController> questionControllers;

    [Space]
    [SerializeField] private int currentQuest;

    [Space]
    [SerializeField] private AudioClip backgroundMusicClip;

    [Space]
    [SerializeField] private GameObject gameplayObj;
    [SerializeField] private GameObject rightObj;
    [SerializeField] private GameObject wrongObj;
    [SerializeField] private GameObject winObj;
    [SerializeField] private GameObject gameOverObj;

    [Space]
    [SerializeField] private Slider healthPlayerSlider;

    [Header("DEBUGGER")]
    [SerializeField] private int questionIndex;
    [SerializeField] private float tempHealth;

    private void OnEnable()
    {
        GameManager.Instance.SoundManager.SetBGMusic(backgroundMusicClip);
        tempHealth = userData.CurrentHealth;
        healthPlayerSlider.value = userData.CurrentHealth / 100;

        StartCoroutine(SetupQuestions());

        GameManager.Instance.SceneController.ActionPass = true;
    }

    IEnumerator SetupQuestions()
    {
        for (int a = 0; a < questionControllers.Count; a++)
        {
            questionControllers[a].SetupAnswerItems(questionDatas[a].Answers, questionDatas[a].Question);

            yield return null;
        }

        yield return null;
    }

    public void CheckAnswer(string answer)
    {
        bool isRight = false;

        if (answer == questionDatas[questionIndex].RightAnswer)
        {
            isRight = true;

            if (userData.PlayerStatistics.ContainsKey("stageFourRightAnswers"))
                userData.PlayerStatistics["stageFourRightAnswers"] += 1;
            else
                userData.PlayerStatistics.Add("stageFourRightAnswers", 1);
        }
        else
        {
            //  DAMGE HERE
            userData.CurrentHealth -= 34f;

            if (userData.CurrentHealth <= 0f)
                userData.CurrentHealth = 0f;

            healthPlayerSlider.value = userData.CurrentHealth / 100f;

            if (userData.PlayerStatistics.ContainsKey("stageFourWrongAnswers"))
                userData.PlayerStatistics["stageFourWrongAnswers"] += 1;
            else
                userData.PlayerStatistics.Add("stageFourWrongAnswers", 1);
        }

        if (QuestionIndex < questionDatas.Count)
            QuestionIndex++;

        StartCoroutine(ShowRightWrongPanel(isRight));
    }

    IEnumerator ShowRightWrongPanel(bool isRight)
    {
        gameplayObj.SetActive(false);

        if (isRight)
            rightObj.SetActive(true);
        else
            wrongObj.SetActive(true);

        yield return new WaitForSeconds(2f);

        rightObj.SetActive(false);
        wrongObj.SetActive(false);

        if (userData.CurrentHealth <= 0f)
        {
            //  gameOver

            gameOverObj.SetActive(true);

            yield break;
        }

        gameplayObj.SetActive(true);
    }


    public void RestartScene()
    {
        userData.CurrentHealth = tempHealth;
        GameManager.Instance.SceneController.RestartScene();
    }

    public void ReturnSchoolGameOver()
    {
        userData.CurrentHealth = tempHealth;
        GameManager.Instance.SceneController.CurrentScene = "School";
    }

    public void ReturnSchoolWin()
    {
        if (schoolSceneData.QuestIndex == currentQuest)
            schoolSceneData.QuestIndex++;

        GameManager.Instance.SceneController.CurrentScene = "School";
    }

    public void ResumeWorldTime(bool value)
    {
        if (value)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
    }
}
