using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageOneController : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private SchoolSceneData schoolSceneData;
    [SerializeField] private PlayerDeath playerDeath;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CanvasGroup gameOverCG;

    [Header("TIMER")]
    [SerializeField] private float startTime;
    [SerializeField] private TextMeshProUGUI hiraganaTimer;
    [SerializeField] private TextMeshProUGUI timer;

    [Header("DEBUGGER")]
    [SerializeField] private float currentTime;
    [SerializeField] private float temphealth;
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
                hiraganaTimer.text = $"Time left: {minutes:D2} : {seconds:D2}";
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

    public void ResumeWorldTime(bool value)
    {
        if (value)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverCG.alpha = 0f;
        gameOverCG.gameObject.SetActive(true);
        LeanTween.alphaCanvas(gameOverCG, 1f, 0.25f).setEase(LeanTweenType.easeInOutSine);
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
