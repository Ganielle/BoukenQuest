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
    [SerializeField] public AudioClip answerClip;

    public void CheckAnswer()
    {
        animator.enabled = true;
        sceneController.CheckAnswer(answer);
    }
}
