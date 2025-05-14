using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stage4PlayerEnvironmentDetector : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private GamePlayerController playerController;
    [SerializeField] private TextToSpeechController textToSpeechController;
    [SerializeField] private Button interactBtn;
    [SerializeField] private Button questionSpeechBtn;
    [SerializeField] private Button answerSpeechBtn;

    [Space]
    [SerializeField] private GameObject answerPanelObj;
    [SerializeField] private TextMeshProUGUI answerTMP;

    [Space]
    [SerializeField] private GameObject questionPanelObj;
    [SerializeField] private TextMeshProUGUI questionTMP;

    [Space]
    [SerializeField] private GameObject gameplayObj;
    [SerializeField] private GameObject winObj;

    [Header("DEBUGGER")]
    [SerializeField] private Stage4AnswerItem stage4AnswerItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("QuestionQuest"))
        {
            stage4AnswerItem = other.GetComponent<Stage4AnswerItem>();
            answerTMP.text = stage4AnswerItem.answer;

            answerSpeechBtn.onClick.RemoveAllListeners();
            answerSpeechBtn.onClick.AddListener(() => textToSpeechController.PlaySpeech(stage4AnswerItem.answerClip));

            answerPanelObj.SetActive(true);
        }
        else if (other.CompareTag("Stage4Question"))
        {
            questionTMP.text = other.GetComponent<Stage4QuestionItem>().question;

            questionSpeechBtn.onClick.RemoveAllListeners();
            questionSpeechBtn.onClick.AddListener(() => textToSpeechController.PlaySpeech(other.GetComponent<Stage4QuestionItem>().questionClip));

            questionPanelObj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("QuestionQuest"))
        {
            stage4AnswerItem = null;
            answerPanelObj.SetActive(false);
            answerTMP.text = "";
        }
        else if (other.CompareTag("Stage4Question"))
        {
            questionPanelObj.SetActive(false);
            questionTMP.text = "";
        }
        else if (other.CompareTag("Teleporter"))
        {
            userData.CurrentMoney += 50;
            Time.timeScale = 0f;
            winObj.SetActive(true);
            gameplayObj.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerController.Interact)
        {
            if (stage4AnswerItem == null) return;

            stage4AnswerItem.CheckAnswer();

            stage4AnswerItem = null;
            answerPanelObj.SetActive(false);
            answerTMP.text = "";

            playerController.InteractTurnOff();
        }

        if (stage4AnswerItem != null) interactBtn.interactable = true;
        else interactBtn.interactable = false;
    }
}
