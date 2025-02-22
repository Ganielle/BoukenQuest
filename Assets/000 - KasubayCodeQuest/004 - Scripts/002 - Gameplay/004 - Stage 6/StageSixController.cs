using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSixController : MonoBehaviour
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

    //  ======================

    [SerializeField] private UserData userData;
    [SerializeField] private SchoolSceneData schoolSceneData;

    [Space]
    [SerializeField] private int currentQuest;

    [Space]
    [SerializeField] private List<Stage3QuestionData> questions;
    [SerializeField] private List<StageSixQuestionController> platformControllers;

    [Space]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CanvasGroup gameOverCG;
    [SerializeField] private CanvasGroup winObject;

    [SerializeField] private Transform playerTF;

    [Space]
    [SerializeField] private GameObject questionObj;
    [SerializeField] private TextMeshProUGUI questionTMP;

    [Header("DEBUGGER")]
    [SerializeField] private int questionIndex;
    public Vector3 CurrentRestartPosition;
    [SerializeField] private float temphealth;

    private void Awake()
    {
        GameManager.Instance.SceneController.AddActionLoadinList(Shuffler.Shuffle(questions));
        GameManager.Instance.SceneController.AddActionLoadinList(InitializePlatforms());

        OnQuestionIndexChange += QuestionChange;

        temphealth = userData.CurrentHealth;
        CurrentRestartPosition = playerTF.position;
        CheckHealth();
        userData.OnHealhChange += HealthChange;

        GameManager.Instance.SceneController.ActionPass = true;

        QuestionShow();
    }

    private void OnDisable()
    {
        OnQuestionIndexChange -= QuestionChange;
        userData.OnHealhChange -= HealthChange;
    }

    private void QuestionChange(object sender, EventArgs e)
    {
        QuestionShow();
    }
    private void HealthChange(object sender, EventArgs e)
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        healthSlider.value = userData.CurrentHealth / 100f;

        if (userData.CurrentHealth <= 0f)
        {
            GameOver();
        }
        else
        {
            playerTF.position = CurrentRestartPosition;
        }
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverCG.alpha = 0f;
        gameOverCG.gameObject.SetActive(true);
        LeanTween.alphaCanvas(gameOverCG, 1f, 0.25f).setEase(LeanTweenType.easeInOutSine);
    }

    private void QuestionShow()
    {
        questionTMP.text = questions[questionIndex].Question;
        questionObj.SetActive(true);

        CurrentRestartPosition = playerTF.position;
    }

    IEnumerator InitializePlatforms()
    {
        for (int a = 0; a < platformControllers.Count; a++)
        {
            int rand = UnityEngine.Random.Range(0, 2);
            platformControllers[a].InitializePlatform(rand == 0, rand == 1, questions[a].RightAnswer, questions[a].RightAnswer == "CORRECT" ? "INCORRECT" : "CORRECT", a);
            yield return null;
        }
    }


    public void Restart()
    {
        userData.CurrentHealth = temphealth;
        GameManager.Instance.SceneController.RestartScene();
    }

    public void ReturnToSchoolDead()
    {
        userData.CurrentHealth = temphealth;
        GameManager.Instance.SceneController.CurrentScene = "School";
    }

    public void ReturnToSchoolSuccess()
    {
        if (schoolSceneData.QuestIndex == currentQuest)
            schoolSceneData.QuestIndex++;

        GameManager.Instance.SceneController.CurrentScene = "School";
    }

    public void WinPlayer()
    {
        userData.CurrentMoney += 50;
        Time.timeScale = 0f;
        winObject.alpha = 0f;
        winObject.gameObject.SetActive(true);
        LeanTween.alphaCanvas(winObject, 1f, 0.25f).setEase(LeanTweenType.easeInOutSine);
    }

    public void PauseResumeGame(float value) => Time.timeScale = value;
}
