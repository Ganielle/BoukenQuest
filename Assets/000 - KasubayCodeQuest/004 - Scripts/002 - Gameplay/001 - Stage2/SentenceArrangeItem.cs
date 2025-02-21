using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SentenceArrangeItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answer;
    [SerializeField] private TextMeshProUGUI translate;
    [SerializeField] private TextMeshProUGUI index;

    [Header("DEBUGGER")]
    public int currentIndex;

    public void SetData(string answer, string translate)
    {
        if (answer == "")
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        this.answer.text = answer;
        this.translate.text = translate;
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
}
