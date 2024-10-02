using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemController : MonoBehaviour
{
    private event EventHandler BattleAnswerChange;
    public event EventHandler OnBattleAnswerChange
    {
        add
        {
            if (BattleAnswerChange == null || !BattleAnswerChange.GetInvocationList().Contains(value))
                BattleAnswerChange += value;
        }
        remove { BattleAnswerChange -= value; }
    }
    public BattleAnswerItem CurrentPlayerAnswer
    {
        get => playerAnswer;
        set
        {
            playerAnswer = value;
            BattleAnswerChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  ========================

    [SerializeField] private UserData userData;
    [SerializeField] private SchoolSceneData schoolSceneData;

    [Space]
    [SerializeField] private AudioClip backgroundMusicClip;

    [Space]
    [SerializeField] private List<QuestionData> questionList;
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private List<Animator> enemyAnimators;
    [SerializeField] private List<ParticleSystem> enemyParticleSystem;

    [Space]
    [SerializeField] private GameObject instructionObj;
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private GameObject winObj;

    [Header("PLAYER")]
    [SerializeField] private Transform playerTF;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ParticleSystem playerAura;
    [SerializeField] private Slider playerHealthSlider;

    [Header("PANELS")]
    [SerializeField] private GameObject questionObj;
    [SerializeField] private GameObject answersObj;
    [SerializeField] private GameObject viewCodeObj;
    [SerializeField] private GameObject codeObj;
    [SerializeField] private GameObject choicesPanelObj;

    [Header("RIGHT WRONG")]
    [SerializeField] private GameObject rightObj;
    [SerializeField] private GameObject wrongObj;
    [SerializeField] private TextMeshProUGUI wrongTipsTMP;

    [Header("QUESTIONS")]
    [SerializeField] private TextMeshProUGUI questionTMP;
    [SerializeField] private TextMeshProUGUI codeQuestionTMP;
    [SerializeField] private List<BattleAnswerItem> answerItems;

    [Header("DEBUGGER")]
    [SerializeField] private int currentBattleIndex;
    [SerializeField] private BattleAnswerItem playerAnswer;
    [SerializeField] private float playerCurrentHealth;
    [SerializeField] private float tempCurrentHealh;

    private void OnEnable()
    {
        GameManager.Instance.SoundManager.SetBGMusic(backgroundMusicClip);
        tempCurrentHealh = userData.CurrentHealth;
        playerCurrentHealth = userData.CurrentHealth;
        OnBattleAnswerChange += PlayerAnswerChange;
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void OnDisable()
    {
        OnBattleAnswerChange -= PlayerAnswerChange;
    }

    private void PlayerAnswerChange(object sender, EventArgs e)
    {
        if (CurrentPlayerAnswer != null) CheckAnswer();
    }

    private void PanelCleanUp()
    {
        choicesPanelObj.SetActive(false);
        questionObj.SetActive(false);
        answersObj.SetActive(false);
        viewCodeObj.SetActive(false);
        codeObj.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(StartSetupCurrentBattle(0f));
    }


    public void RestartScene()
    {
        userData.CurrentHealth = tempCurrentHealh;
        GameManager.Instance.SceneController.RestartScene();
    }

    public void ReturnSchoolGameOver()
    {
        userData.CurrentHealth = tempCurrentHealh;
        GameManager.Instance.SceneController.CurrentScene = "School";
    }

    public void ReturnSchoolWin()
    {
        schoolSceneData.QuestIndex++;
        GameManager.Instance.SceneController.CurrentScene = "School";
    }

    public void NextEnemy()
    {
        currentBattleIndex++;

        if (currentBattleIndex >= enemyList.Count)
        {
            //  WIN HERE
            return;
        }

        enemyList[currentBattleIndex - 1].SetActive(false);
        enemyList[currentBattleIndex].SetActive(true);

        StartCoroutine(StartSetupCurrentBattle(2.5f));
    }

    IEnumerator StartSetupCurrentBattle(float waitTime)
    {
        PanelCleanUp();

        SetupQuestion();

        yield return new WaitForSeconds(waitTime);

        questionObj.SetActive(true);
        choicesPanelObj.SetActive(true);
        answersObj.SetActive(true);
    }

    private void SetupQuestion()
    {
        if (questionList[currentBattleIndex].HaveCodeQuestion)
        {
            viewCodeObj.SetActive(true);
            codeQuestionTMP.text = questionList[currentBattleIndex].CodeQuestion;
        }
        else
        {
            viewCodeObj.SetActive(false);
            codeQuestionTMP.text = "";
        }

        questionTMP.text = questionList[currentBattleIndex].Question;

        for (int a = 0; a < questionList[currentBattleIndex].Answers.Count; a++)
        {
            answerItems[a].SetupData(questionList[currentBattleIndex].Answers[a]);
        }
    }

    private void CheckAnswer()
    {
        PanelCleanUp();

        if (CurrentPlayerAnswer.Answer == questionList[currentBattleIndex].RightAnswer)
        {
            rightObj.SetActive(true);

            if (userData.PlayerStatistics.ContainsKey("stageFiveRightAnswers"))
                userData.PlayerStatistics.Add("stageFiveRightAnswers", 1);
            else
                userData.PlayerStatistics["stageFiveRightAnswers"] += 1;
        }
        else
        {
            wrongTipsTMP.text = questionList[currentBattleIndex].Tips;
            wrongObj.SetActive(true);

            if (userData.PlayerStatistics.ContainsKey("stageFiveWrongAnswers"))
                userData.PlayerStatistics.Add("stageFiveWrongAnswers", 1);
            else
                userData.PlayerStatistics["stageFiveWrongAnswers"] += 1;
        }
    }

    public void NextPhaseAfterAttack()
    {
        if (CurrentPlayerAnswer.Answer == questionList[currentBattleIndex].RightAnswer)
        {
            enemyAnimators[currentBattleIndex].SetTrigger("death");
        }
        else
        {
            enemyAnimators[currentBattleIndex].SetTrigger("casting");
            enemyParticleSystem[currentBattleIndex].Play();
        }
    }

    public void NexPhaseEnemyToPlayer()
    {
        playerCurrentHealth -= 34;
        userData.CurrentHealth = playerCurrentHealth;

        playerHealthSlider.value = playerCurrentHealth / 100;

        if (playerCurrentHealth <= 0)
        {
            playerAnimator.SetTrigger("death");
        }
        else
        {
            StartCoroutine(StartSetupCurrentBattle(1f));
        }
    }

    public void GameOverPlayer() => gameOverObj.SetActive(true);

    public void WinPlayer()
    {
        if (currentBattleIndex >= enemyList.Count)
        {
            userData.CurrentMoney += 50;
            winObj.SetActive(true);
            return;
        }
    }

    public Transform EnemyTF() => enemyList[currentBattleIndex].transform;
    public Transform PlayerTF() => playerTF;

    public bool IsRightPlayer() => CurrentPlayerAnswer.Answer == questionList[currentBattleIndex].RightAnswer;

    public void ReadyMagicCastingPlayer()
    {
        playerAnimator.SetTrigger("startcast");
        playerAura.Play();
    }

    public void ContinueAfterRightOrWrong()
    {
        rightObj.SetActive(false);
        wrongObj.SetActive(false);
        playerAnimator.SetTrigger("casting");
    }
}
