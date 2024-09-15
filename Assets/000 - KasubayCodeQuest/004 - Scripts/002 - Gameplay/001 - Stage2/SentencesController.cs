using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentencesController : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private SchoolSceneData schoolSceneData;

    [Space]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CanvasGroup gameOverCG;
    [SerializeField] private CanvasGroup winObject;
    [SerializeField] private Button interactBtn;

    [Space]
    [SerializeField] private List<SentenceArrangeItem> answerItems;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private TextMeshProUGUI objectives;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject wrong;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI questionTimer;
    [SerializeField] private float startTime;

    [Header("DEBUGGER")]
    [SerializeField] private SentencesData currentData;
    [SerializeField] private int currentObjectives;
    [SerializeField] private float temphealth;
    [SerializeField] private float currentTime;
    [SerializeField] private bool startGame;

    private void OnEnable()
    {
        temphealth = userData.CurrentHealth;
        CheckHealth();
        currentTime = startTime;
        userData.OnHealhChange += HealthChange;
        startGame = true;

        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void OnDisable()
    {
        userData.OnHealhChange -= HealthChange;
    }

    private void Update()
    {
        if (startGame)
        {
            if (currentTime > 0f)
            {
                currentTime -= Time.deltaTime;

                int minutes = (int)currentTime / 60;
                int seconds = (int)currentTime % 60;

                timer.text = $"{minutes:D2} : {seconds:D2}";
                questionTimer.text = $"Time left: {minutes:D2} : {seconds:D2}";
            }
            else
            {
                if (!gameOverCG.gameObject.activeInHierarchy)
                {
                    GameOver();
                }
            }
        }
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
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverCG.alpha = 0f;
        gameOverCG.gameObject.SetActive(true);
        LeanTween.alphaCanvas(gameOverCG, 1f, 0.25f).setEase(LeanTweenType.easeInOutSine);
    }

    public void SetupQuestionData(SentencesData data)
    {
        currentData = data;

        gameplayPanel.SetActive(false);

        StartCoroutine(SetQDatas(data));
    }

    private IEnumerator SetQDatas(SentencesData data)
    {
        List<Sentences> tempDatas = new List<Sentences>();

        tempDatas = data.SentencesList;

        yield return StartCoroutine(tempDatas.Shuffle());

        for (int a = 0; a < answerItems.Count; a++)
        {
            if (a < data.SentencesList.Count)
                answerItems[a].SetData(tempDatas[a].characters, tempDatas[a].translation);
            else
                answerItems[a].SetData("", "");

            yield return null;
        }

        questionPanel.SetActive(true);
    }

    public void CheckAnswer()
    {
        bool canDamage = false;

        interactBtn.interactable = false;

        for (int a = 0; a < answerItems.Count; a++)
        {
            if (a >= currentData.SentencesList.Count) break;

            if (answerItems[a].currentIndex.ToString() != currentData.SentencesList[a].index)
            {
                canDamage = true;
                break;
            }
        }

        if (canDamage)
        {
            userData.CurrentHealth -= 20;
            wrong.SetActive(true);
        }
        else
            right.SetActive(true);

        if (currentObjectives < 10)
        {
            currentObjectives++;
            objectives.text = $"Statues Answered:\n {currentObjectives} / 10";
        }

        currentData = null;

        StartCoroutine(RightWrongIcon());
    }

    private IEnumerator RightWrongIcon()
    {
        yield return new WaitForSeconds(1.2f);

        wrong.SetActive(false);
        right.SetActive(false);

        if (currentObjectives >= 10)
        {
            startGame = false;
            Time.timeScale = 0f;
            winObject.alpha = 0f;
            winObject.gameObject.SetActive(true);
            LeanTween.alphaCanvas(winObject, 1f, 0.25f).setEase(LeanTweenType.easeInOutSine);
            yield break;
        }

        questionPanel.SetActive(false);
        gameplayPanel.SetActive(true);
    }

    public void ResumeWorldTime(bool value)
    {
        if (value)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
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
        schoolSceneData.QuestIndex++;
        GameManager.Instance.SceneController.CurrentScene = "School";
    }
}
