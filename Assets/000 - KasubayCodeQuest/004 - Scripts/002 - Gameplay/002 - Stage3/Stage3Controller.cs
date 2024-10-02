using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stage3Controller : MonoBehaviour
{
    private event EventHandler AnswerItemChange;
    public event EventHandler OnAnswerItemChange
    {
        add
        {
            if (AnswerItemChange == null || !AnswerItemChange.GetInvocationList().Contains(value))
                AnswerItemChange += value;
        }
        remove { AnswerItemChange -= value; }
    }
    public Stage3AnswerItem SelectedAnswer
    {
        get => selectedAnswer;
        set
        {
            selectedAnswer = value;
            AnswerItemChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  =====================

    [SerializeField] private UserData userData;
    [SerializeField] private SchoolSceneData schoolSceneData;
    [SerializeField] private Slider healthSlider;

    [Space]
    [SerializeField] private AudioClip backgroundMusicClip;

    [Space]
    [SerializeField] private List<GameObject> cameras;

    [Space]
    [SerializeField] private TextMeshProUGUI questionTMP;
    [SerializeField] private TextMeshProUGUI questionCounterTMP;
    [SerializeField] private List<Stage3AnswerItem> answers;
    [SerializeField] private List<Stage3QuestionData> questionDatas;
    [SerializeField] private Button checkAnswerBtn;

    [Space]
    [SerializeField] private GameObject instructionObj;
    [SerializeField] private GameObject questionPanelObj;
    [SerializeField] private GameObject rightAnswerObj;
    [SerializeField] private GameObject wrongAnswerObj;
    [SerializeField] private GameObject youWinObj;
    [SerializeField] private GameObject gameOverObj;

    [Header("DEBUGGER")]
    [SerializeField] private Stage3AnswerItem selectedAnswer;
    [SerializeField] private bool isPlaying;
    [SerializeField] private int questionIndex;
    [SerializeField] private float tempPlayerHealth;
    [SerializeField] private float currentCameraTimer;
    [SerializeField] private int cameraIndex;

    private void OnEnable()
    {
        GameManager.Instance.SoundManager.SetBGMusic(backgroundMusicClip);
        tempPlayerHealth = userData.CurrentHealth;
        healthSlider.value = userData.CurrentHealth / 100f;

        currentCameraTimer = 5f;

        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (currentCameraTimer <= 0f)
            {
                cameras[cameraIndex].SetActive(false);

                if (cameraIndex >= cameras.Count - 1f)
                    cameraIndex = 0;
                else
                    cameraIndex++;

                cameras[cameraIndex].SetActive(true);

                currentCameraTimer = 5f;
            }
            else
            {
                currentCameraTimer -= Time.deltaTime;
            }
        }
    }


    IEnumerator SetupQuestions()
    {
        SelectedAnswer = null;


        rightAnswerObj.SetActive(false);
        wrongAnswerObj.SetActive(false);

        questionCounterTMP.text = $"{(questionIndex + 1)} out of 10";
        questionTMP.text = questionDatas[questionIndex].Question;

        for (int a = 0; a < questionDatas[questionIndex].Answers.Count; a++)
        {
            answers[a].answer = questionDatas[questionIndex].Answers[a];
            yield return null;
        }

        questionPanelObj.SetActive(true);
    }

    public void CheckAnswer()
    {
        if (selectedAnswer == null) return;

        bool isRight = false;

        if (selectedAnswer.answer == questionDatas[questionIndex].RightAnswer)
        {
            //  ADD CODE HERE ?
            isRight = true;
        }
        else
        {
            //  DAMAGE HERE
            userData.CurrentHealth -= 34f;

            if (userData.CurrentHealth <= 0f)
                userData.CurrentHealth = 0f;

            healthSlider.value = userData.CurrentHealth / 100f;
        }

        questionIndex++;
        StartCoroutine(ShowRightWrongPanel(isRight));
    }

    IEnumerator ShowRightWrongPanel(bool isRight)
    {
        questionPanelObj.SetActive(false);

        if (isRight)
            rightAnswerObj.SetActive(true);
        else
            wrongAnswerObj.SetActive(true);

        yield return new WaitForSeconds(2f);

        rightAnswerObj.SetActive(false);
        wrongAnswerObj.SetActive(false);


        if (questionIndex >= questionDatas.Count - 1)
        {
            //  WIN

            youWinObj.SetActive(true);

            yield break;
        }

        if (userData.CurrentHealth <= 0f)
        {
            //  gameOver

            gameOverObj.SetActive(true);

            yield break;
        }
        StartCoroutine(SetupQuestions());
    }

    public void StartGame()
    {
        instructionObj.SetActive(false);

        isPlaying = true;

        StartCoroutine(SetupQuestions());
    }

    public void RestartScene()
    {
        userData.CurrentHealth = tempPlayerHealth;
        GameManager.Instance.SceneController.RestartScene();
    }

    public void ReturnSchoolGameOver()
    {
        userData.CurrentHealth = tempPlayerHealth;
        GameManager.Instance.SceneController.CurrentScene = "School";
    }

    public void ReturnSchoolWin()
    {
        schoolSceneData.QuestIndex++;
        GameManager.Instance.SceneController.CurrentScene = "School";
    }
}
