using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentencesController : MonoBehaviour
{
    [SerializeField] private UserData userData;

    [Space]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CanvasGroup gameOverCG;

    [Space]
    [SerializeField] private List<SentenceArrangeItem> answerItems;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private TextMeshProUGUI objectives;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject wrong;

    [Header("DEBUGGER")]
    [SerializeField] private SentencesData currentData;
    [SerializeField] private int currentObjectives;

    private void OnEnable()
    {
        CheckHealth();
        userData.OnHealhChange += HealthChange;
    }

    private void OnDisable()
    {
        userData.OnHealhChange -= HealthChange;
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

        StartCoroutine(RightWrongIcon());
    }

    private IEnumerator RightWrongIcon()
    {
        yield return new WaitForSeconds(1.2f);

        wrong.SetActive(false);
        right.SetActive(false);

        questionPanel.SetActive(false);
        gameplayPanel.SetActive(true);
    }
}
