using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PairPopCards : MonoBehaviour
{
    public Sprite FrontCard
    {
        get => frontCard;
        set => frontCard = value;
    }

    public int CardNumber
    {
        get => cardNumber;
        set => cardNumber = value;
    }

    public bool IsFlipped
    {
        get => isFlipped;
        set => isFlipped = value;
    }

    public TextMeshPro CardAnswer
    {
        get => cardAnswer;
    }

    //  ===========================

    [SerializeField] private PairPopController controller;
    [SerializeField] private Sprite backCard;
    [SerializeField] private SpriteRenderer cardSR;
    [SerializeField] private Animator cardAnimator;
    [SerializeField] private TextMeshPro cardAnswer;

    [Header("DEBUGGER")]
    [SerializeField] private int cardNumber;
    [SerializeField] private Sprite frontCard;
    [SerializeField] private bool isBackCard;
    [SerializeField] private bool isFlipped;
    [SerializeField] private bool isFlipping;

    private void OnEnable()
    {
        controller.OnPairPopStateChange += StateChange;
    }

    private void OnDisable()
    {
        controller.OnPairPopStateChange -= StateChange;
    }

    private void StateChange(object sender, EventArgs e)
    {
        if (controller.CurrentPairPopState == PairPopController.PairPopState.GAMEPLAY)
        {
            StartCoroutine(ShowPairsOnStart());
        }
    }

    IEnumerator ShowPairsOnStart()
    {
        cardAnimator.SetTrigger("flip");

        yield return new WaitForSecondsRealtime(5f);

        cardAnimator.SetTrigger("flipToBack");

        yield return new WaitForSeconds(1f);

        ResetAnimation();
    }

    public void PlayCardAnimator(string animName) => cardAnimator.SetTrigger(animName);

    public void FlipCardFront()
    {
        if (isFlipped) return;

        isFlipped = true;

        cardAnimator.SetTrigger("flip");
    }

    public void FlipCardBack()
    {
        cardAnimator.SetTrigger("flipToBack");
    }

    public void SetFlippedToFalse() => isFlipped = false;

    public void ResetAnimation()
    {
        cardAnimator.ResetTrigger("flip");
        cardAnimator.ResetTrigger("flipToBack");
    }

    public void ChangeToFrontCard() => cardSR.sprite = frontCard;

    public void ChangeToBackCard() => cardSR.sprite = backCard;
}
