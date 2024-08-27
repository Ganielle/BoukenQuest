using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageOneController : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private PlayerDeath playerDeath;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CanvasGroup gameOverCG;

    [Header("TIMER")]
    [SerializeField] private float startTime;
    [SerializeField] private TextMeshProUGUI hiraganaTimer;
    [SerializeField] private TextMeshProUGUI timer;

    [Header("DEBUGGER")]
    [SerializeField] private float currentTime;

    private void OnEnable()
    {
        CheckHealth();
        currentTime = startTime;
        userData.OnHealhChange += HealthChange;
    }

    private void OnDisable()
    {
        userData.OnHealhChange -= HealthChange;
    }

    private void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            int minutes = (int)currentTime / 60;
            int seconds = (int)currentTime % 60;

            timer.text = $"{minutes:D2} : {seconds:D2}";
            hiraganaTimer.text = $"Time left: {minutes:D2} : {seconds:D2}";
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
}
