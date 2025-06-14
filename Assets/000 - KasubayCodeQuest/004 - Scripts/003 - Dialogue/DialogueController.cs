using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private string characterName;
    [SerializeField] private bool isItem;
    [SerializeField] private GameObject itemObject;
    [SerializeField] private Button interactionButton;

    [Space]
    [SerializeField] private bool isEnding;
    [SerializeField] private GameObject endingPanel;

    [Space]
    [SerializeField] private bool isLesson;
    [SerializeField] private GameObject lessonObj;


    [Space]
    [SerializeField] private NPCQuestController npcQuestController;
    [SerializeField] private QuestController questController;
    [SerializeField] private GamePlayerController playerControls;
    [SerializeField] private float typeSpeed;
    [SerializeField] private Image blackPanel;

    [Header("ANIMATOR")]
    [SerializeField] private bool isUsingAnimator;
    [SerializeField] private Animator npcAnimator;

    [Space]
    [SerializeField] private GameObject dialogueObj;
    [SerializeField] private GameObject gameplayUIObj;
    [SerializeField] private TextMeshProUGUI characterNameTMP;
    [SerializeField] private TextMeshProUGUI dialogueTMP;
    [SerializeField] private Button nextBtn;

    [Header("DEBUGGER")]
    [SerializeField] private bool canPressSkip;
    [SerializeField] private bool canNextDialogue;
    [SerializeField] private int currentDialogueIndex;
    [SerializeField] private DialogueData dialogues;

    //  =========================

    Coroutine showText;
    Action finalAction;

    Coroutine dialogueCoroutine;

    //  =========================

    private void OnEnable()
    {
        CheckQuest();
        questController.OnQuestIndexChange += QuestChange;
    }

    private void OnDisable()
    {
        questController.OnQuestIndexChange -= QuestChange;
    }

    private void QuestChange(object sender, EventArgs e)
    {
        CheckQuest();
    }

    private void CheckQuest()
    {
        dialogues = questController.QuestList[questController.QuestIndex].Dialouges.Find(e => e.characterName == characterName).dialogue;

        if (questController.QuestList[questController.QuestIndex].Positions.Exists(e => e.characterName == characterName))
        {
            Vector3 tempquestposition = questController.QuestList[questController.QuestIndex].Positions.Find(e => e.characterName == characterName).position;
            Vector3 temprotation = questController.QuestList[questController.QuestIndex].Positions.Find(e => e.characterName == characterName).rotation;

            transform.position = tempquestposition;
            transform.rotation = Quaternion.Euler(temprotation);
        }
    }

    public void Initialize()
    {
        if (isLesson)
        {
            gameplayUIObj.SetActive(false);
            lessonObj.SetActive(true);
            return;
        }

        if (npcQuestController.CheckIfThisIsQuest() && isEnding)
        {
            gameplayUIObj.SetActive(false);
            endingPanel.SetActive(true);
            return;
        }

        this.finalAction = () =>
        {
            int tempquestindex = questController.QuestIndex + 1;

            if (!isItem)
            {
                if (showText != null)
                    StopCoroutine(showText);

                if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);

                if (!npcQuestController.CheckIfThisIsQuest())
                {
                    blackPanel.gameObject.SetActive(false);
                    gameplayUIObj.SetActive(true);
                    playerControls.Disable = false;
                    return;
                }

                Vector3 tempquestposition = questController.QuestList[tempquestindex].Positions.Find(e => e.characterName == characterName).position;
                Vector3 temprotation = questController.QuestList[tempquestindex].Positions.Find(e => e.characterName == characterName).rotation;

                if (tempquestposition != Vector3.zero)
                {
                    if (transform.position == tempquestposition)
                    {
                        gameplayUIObj.SetActive(true);
                        npcQuestController.ProgressQuest();
                        return;
                    }

                    playerControls.Disable = true;

                    blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, 0f);
                    blackPanel.gameObject.SetActive(true);

                    LeanTween.value(blackPanel.gameObject, (e) => blackPanel.color = e, new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, 0f), new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, 1f), 0.5f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
                    {
                        transform.position = tempquestposition;
                        transform.rotation = Quaternion.Euler(temprotation);
                        LeanTween.value(blackPanel.gameObject, (e) => blackPanel.color = e, new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, 1f), new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, 0f), 0.5f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
                        {
                            blackPanel.gameObject.SetActive(false);
                            gameplayUIObj.SetActive(true);
                            playerControls.Disable = false;

                            npcQuestController.ProgressQuest();
                        });
                    });

                    return;
                }
            }
            else
            {
                if (showText != null)
                    StopCoroutine(showText);

                interactionButton.interactable = false;
                interactionButton.onClick.RemoveAllListeners();

                //  ADD TO INVENTORY

            }


            npcQuestController.ProgressQuest();
            gameplayUIObj.SetActive(true);
        };
        nextBtn.onClick.AddListener(() => NextDialogueBtn());
        gameplayUIObj.SetActive(false);
        dialogueObj.SetActive(true);
        playerControls.Disable = true;
        dialogueCoroutine = StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue()
    {
        if (isUsingAnimator)
            npcAnimator.SetBool("talking", true);

        currentDialogueIndex = 0;

        for (int a = 0; a < dialogues.nameSequence.Count; a++)
        {
            currentDialogueIndex = a;

            canNextDialogue = false;

            characterNameTMP.text = dialogues.nameSequence[a];
            dialogueTMP.text = "";

            showText = StartCoroutine(TypeEffect(dialogues.dialogueSequence[a]));

            while (!canNextDialogue) yield return null;

            yield return null;
        }
    }

    private IEnumerator TypeEffect(string value)
    {   
        foreach(char c in value)
        {
            dialogueTMP.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        showText = null;
    }

    private void NextDialogueBtn()
    {
        if (currentDialogueIndex >= dialogues.dialogueSequence.Count - 1)
        {
            if (showText != null)
                StopCoroutine(showText);

            if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);

            dialogueObj.SetActive(false);

            characterNameTMP.text = "";
            dialogueTMP.text = "";
            nextBtn.onClick.RemoveAllListeners();
            playerControls.Disable = false;

            if (isUsingAnimator)
                npcAnimator.SetBool("talking", false);

            finalAction?.Invoke();
        }
        else
        {
            if (showText != null)
            {
                StopCoroutine(showText);
                showText = null;
            }

            canNextDialogue = true;
        }
    }
}
