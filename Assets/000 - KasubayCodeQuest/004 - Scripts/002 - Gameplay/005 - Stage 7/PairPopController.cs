using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PairPopController : MonoBehaviour
{
    public enum PairPopState
    {
        READY,
        GAMEPLAY,
        GAMEOVER
    }
    private event EventHandler PairPopStateChange;
    public event EventHandler OnPairPopStateChange
    {
        add
        {
            if (PairPopStateChange == null || !PairPopStateChange.GetInvocationList().Contains(value))
                PairPopStateChange += value;
        }
        remove { PairPopStateChange -= value; }
    }
    public PairPopState CurrentPairPopState
    {
        get => currentPairPopState;
        set
        {
            currentPairPopState = value;
            PairPopStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool CanPick
    {
        get => canPick;
        set => canPick = value;
    }

    //  ==========================

    [SerializeField] private SchoolSceneData schoolSceneData;
    [SerializeField] private UserData userData;
    [SerializeField] private int currentQuest;

    [Space]
    [SerializeField] private AudioClip winClip;

    [SerializeField] private List<PairPopData> pairPopDatas;
    [SerializeField] private List<PairPopCards> mainCardList;

    [Header("RAW TEXTURE BOARD")]
    [SerializeField] private Camera boardCamera;
    [SerializeField] private RectTransform boardRT;
    [SerializeField] private RawImage boardTexture;

    [Space]
    [SerializeField] private float startTimer;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private CanvasGroup gameOverCG;
    [SerializeField] private CanvasGroup winObject;

    [Header("DEBUGGER")]
    [SerializeField] private PairPopState currentPairPopState;
    [SerializeField] private List<PairPopData> pairPopDatasTemp;
    [SerializeField] private List<PairPopCards> mainCardListTemp;
    [SerializeField] private bool canPick;
    [SerializeField] private bool insideBoard;
    [SerializeField] Vector2 localPoint;
    [SerializeField] Vector2 viewportClick;
    [SerializeField] Vector2 worldPoint;
    [SerializeField] private PairPopCards firstSelectedCard;
    [SerializeField] private PairPopCards secondSelectedCard;
    [SerializeField] private int score;
    [SerializeField] private bool startGame;
    [SerializeField] private float currentTime;

    //  ===============================

    RaycastHit2D hit;

    //  ===============================

    private void Awake()
    {
        GameManager.Instance.SceneController.AddActionLoadinList(InitializeCards());
        currentTime = startTimer;

        GameManager.Instance.SceneController.ActionPass = true;

        OnPairPopStateChange += StateChange;
    }

    private void OnDisable()
    {
        OnPairPopStateChange -= StateChange;
    }

    private void Update()
    {
        MouseMovement();

        if (startGame)
        {
            if (currentTime > 0f)
            {
                currentTime -= Time.deltaTime;

                int minutes = (int)currentTime / 60;
                int seconds = (int)currentTime % 60;

                timer.text = $"{minutes:D2} : {seconds:D2}";
            }
            else
            {
                if (!gameOverCG.gameObject.activeInHierarchy)
                {
                    GameOver();
                }
            }
        }
        else
        {
            if (CurrentPairPopState == PairPopState.READY)
                timer.text = "GET READY!";
            else if (CurrentPairPopState == PairPopState.GAMEPLAY)
                timer.text = "MEMORIZE THE CARDS AND PICK THE RIGHT PAIR";
        }
    }

    private void StateChange(object sender, EventArgs e)
    {
        if (CurrentPairPopState == PairPopState.GAMEPLAY)
        {
            StartCoroutine(InitializeCardPick());
        }

        ShowReward();
    }
    IEnumerator InitializeCards()
    {
        mainCardListTemp = mainCardList; // Ensure it's a new list
        pairPopDatasTemp = pairPopDatas;
        yield return StartCoroutine(pairPopDatasTemp.Shuffle());
        yield return StartCoroutine(mainCardListTemp.Shuffle());

        int cardindex = 0;
        int maxPairs = Mathf.Min(pairPopDatasTemp.Count, mainCardListTemp.Count / 2);

        for (int i = 0; i < maxPairs; i++)
        {
            int firstCardIndex = i * 2;
            int secondCardIndex = firstCardIndex + 1;

            if (secondCardIndex >= mainCardListTemp.Count) break; // Prevent index out of range

            mainCardListTemp[firstCardIndex].CardAnswer.text = pairPopDatasTemp[i].pairOne;
            mainCardListTemp[firstCardIndex].CardNumber = cardindex;

            mainCardListTemp[secondCardIndex].CardAnswer.text = pairPopDatasTemp[i].pairTwo;
            mainCardListTemp[secondCardIndex].CardNumber = cardindex;

            cardindex++;
        }
    }

    IEnumerator InitializeCardPick()
    {
        yield return new WaitForSecondsRealtime(6.25f);

        CanPick = true;
        startGame = true;
    }

    private void ShowReward()
    {
        if (CurrentPairPopState != PairPopState.GAMEOVER) return;

        //  WIN HERE
        WinPlayer();
    }
    public void WinPlayer()
    {
        userData.CurrentMoney += 50;
        Time.timeScale = 0f;
        winObject.alpha = 0f;
        winObject.gameObject.SetActive(true);
        LeanTween.alphaCanvas(winObject, 1f, 0.25f).setEase(LeanTweenType.easeInOutSine);
    }

    private void MouseMovement()
    {
        if (insideBoard)
        {
            if (!CanPick || CurrentPairPopState != PairPopState.GAMEPLAY) return;

            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(boardRT, Input.mousePosition, GameManager.Instance.UICamera, out localPoint);

            viewportClick = localPoint - boardRT.rect.min;
            viewportClick.y = (boardRT.rect.yMin * -1) - (localPoint.y * -1);
            viewportClick.x *= boardTexture.uvRect.width / boardRT.rect.width;
            viewportClick.y *= boardTexture.uvRect.height / boardRT.rect.height;
            viewportClick += boardTexture.uvRect.min;

            worldPoint = boardCamera.ViewportToWorldPoint(viewportClick);
            hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider == null)
                return;

            if (!hit.collider.CompareTag("Card"))
                return;

            //GameObject clickPoint = Instantiate(pinkPixel);
            //clickPoint.transform.position = new Vector3(worldPoint.x, worldPoint.y,  3f);

            if (firstSelectedCard == null)
            {
                firstSelectedCard = hit.transform.gameObject.GetComponent<PairPopCards>();

                if (firstSelectedCard.IsFlipped)
                {
                    firstSelectedCard = null;
                    return;
                }

                firstSelectedCard.FlipCardFront();
            }
            else
            {
                if (firstSelectedCard != hit.transform.gameObject.GetComponent<PairPopCards>())
                {
                    secondSelectedCard = hit.transform.gameObject.GetComponent<PairPopCards>();

                    if (secondSelectedCard.IsFlipped)
                    {
                        secondSelectedCard = null;
                        return;
                    }

                    secondSelectedCard.FlipCardFront();
                    StartCoroutine(CheckCard());
                }
            }
        }
    }

    IEnumerator CheckCard()
    {
        canPick = false;

        if (firstSelectedCard == null || secondSelectedCard == null)
        {
            canPick = true;
            yield break;
        }

        yield return new WaitForSecondsRealtime(1f);

        if (firstSelectedCard.CardNumber == secondSelectedCard.CardNumber)
        {
            score += 2;

            if (userData.PlayerStatistics.ContainsKey("stageSevenRightAnswers"))
                userData.PlayerStatistics["stageSevenRightAnswers"] += 1;
            else
                userData.PlayerStatistics.Add("stageSevenRightAnswers", 1);

            if (score >= mainCardList.Count)
            {
                score = mainCardList.Count;
                CurrentPairPopState = PairPopState.GAMEOVER;
            }
        }
        else
        {
            firstSelectedCard.FlipCardBack();
            secondSelectedCard.FlipCardBack();

            if (userData.PlayerStatistics.ContainsKey("stageSevenWrongAnswers"))
                userData.PlayerStatistics["stageSevenWrongAnswers"] += 1;
            else
                userData.PlayerStatistics.Add("stageSevenWrongAnswers", 1);
        }

        firstSelectedCard = null;
        secondSelectedCard = null;

        canPick = true;
    }

    public void ChangePairPopState(int index)
    {
        switch (index)
        {
            case (int)PairPopState.READY:
                CurrentPairPopState = PairPopState.READY;
                break;
            case (int)PairPopState.GAMEPLAY:
                CurrentPairPopState = PairPopState.GAMEPLAY;
                break;
            case (int)PairPopState.GAMEOVER:
                CurrentPairPopState = PairPopState.GAMEOVER;
                break;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverCG.alpha = 0f;
        gameOverCG.gameObject.SetActive(true);
        LeanTween.alphaCanvas(gameOverCG, 1f, 0.25f).setEase(LeanTweenType.easeInOutSine);
    }

    public void InsideBoardMouse(bool value) => insideBoard = value;

    public void PauseResumeGame(float value) => Time.timeScale = value;

    public void Restart()
    {
        GameManager.Instance.SceneController.RestartScene();
    }

    public void ReturnToSchoolDead()
    {
        GameManager.Instance.SceneController.CurrentScene = "School";
    }

    public void ReturnToSchoolSuccess()
    {
        if (schoolSceneData.QuestIndex == currentQuest)
            schoolSceneData.QuestIndex++;

        GameManager.Instance.SceneController.CurrentScene = "School";
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(worldPoint, Vector3.forward * 10f);
    }
}
