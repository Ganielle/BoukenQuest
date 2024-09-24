using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stage4AnswerItem : MonoBehaviour
{
    [SerializeField] private Stage4SceneController sceneController;
    [SerializeField] private Animator animator;

    [Header("DEBUGGER")]
    [SerializeField] public string answer;

    public void CheckAnswer()
    {
        animator.enabled = true;
        sceneController.CheckAnswer(answer);
    }
}
