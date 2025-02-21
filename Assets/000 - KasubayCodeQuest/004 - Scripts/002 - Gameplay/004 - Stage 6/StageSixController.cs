using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageSixController : MonoBehaviour
{
    private event EventHandler QuestionIndexChange;
    public event EventHandler OnQuestionIndexChange
    {
        add
        {
            if (QuestionIndexChange == null || !QuestionIndexChange.GetInvocationList().Contains(value))
                QuestionIndexChange += value;
        }
        remove { QuestionIndexChange -= value; }
    }
    public int QuestionIndex
    {
        get => questionIndex;
        set
        {
            questionIndex = value;
            QuestionIndexChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  ======================

    [SerializeField] private List<Stage3QuestionData> questions;
    [SerializeField] private List<StageSixQuestionController> platformControllers;

    [Header("DEBUGGER")]
    [SerializeField] private int questionIndex;

    private void Awake()
    {
        GameManager.Instance.SceneController.AddActionLoadinList(Shuffler.Shuffle(questions));
        GameManager.Instance.SceneController.AddActionLoadinList(InitializePlatforms());
        GameManager.Instance.SceneController.ActionPass = true;
    }

    IEnumerator InitializePlatforms()
    {
        for (int a = 0; a < platformControllers.Count; a++)
        {
            int rand = UnityEngine.Random.Range(0, 2);
            platformControllers[a].InitializePlatform(rand == 0, rand == 1, questions[a].RightAnswer, questions[a].RightAnswer == "CORRECT" ? "CORRECT" : "INCORRECT", a);
            yield return null;
        }
    }

    public void QuestionProgress()
    {

    }
}
