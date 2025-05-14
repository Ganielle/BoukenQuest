using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SentenceArrangeItem : MonoBehaviour
{
    [SerializeField] private TextToSpeechController controller;
    [SerializeField] private TextMeshProUGUI answer;
    [SerializeField] private TextMeshProUGUI translate;
    [SerializeField] private TextMeshProUGUI index;

    [Header("DEBUGGER")]
    public int currentIndex;
    public AudioClip textToSpeech;

    public void SetData(string answer, string translate, AudioClip textToSpeech)
    {
        Debug.Log(answer == "");
        if (answer == "")
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        this.answer.text = answer;
        this.translate.text = translate;
        this.textToSpeech = textToSpeech;
        currentIndex = 1;

        index.text = currentIndex.ToString();
    }

    public void NextBtn()
    {
        if (currentIndex >= 10)
        {
            currentIndex = 1;
        }
        else
        {
            currentIndex++;
        }

        index.text = currentIndex.ToString();   
    }

    public void PreviousBtn()
    {
        if (currentIndex <= 1)
            currentIndex = 10;
        else
            currentIndex--;

        index.text = currentIndex.ToString();
    }

    public void PlaySpeech() => controller.PlaySpeech(textToSpeech);
}
