using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    [SerializeField] private CanvasGroup winCG;
    [SerializeField] private UserData userData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Teleporter"))
        {
            userData.CurrentMoney += 50;
            Time.timeScale = 0f;
            winCG.alpha = 0f;
            winCG.gameObject.SetActive(true);
            LeanTween.alphaCanvas(winCG, 1f, 0.25f).setEase(LeanTweenType.easeInOutSine);
        }
    }
}
