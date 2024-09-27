using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIBtnController : MonoBehaviour
{
    [SerializeField] private Image btnImg;
    [SerializeField] private Color selected;
    [SerializeField] private Color unselected;

    private void OnEnable()
    {
        HoverExit();
    }

    public void HoverEnter()
    {
        btnImg.color = selected;
    }

    public void HoverExit()
    {
        btnImg.color = unselected;
    }
}
