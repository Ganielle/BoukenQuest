using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    [SerializeField] private AudioClip btnClip;

    [Header("GameObject")]
    [SerializeField] public GameObject errorPanelObj;
    [SerializeField] public GameObject confirmationPanelObj;
    [SerializeField] private TextMeshProUGUI errorTMP;
    [SerializeField] private TextMeshProUGUI confirmationTMP;

    [Header("CONGRATULATIONS")]
    [SerializeField] private GameObject congratsOkObj;
    [SerializeField] private GameObject congratsOkCancelObj;
    [SerializeField] private TextMeshProUGUI congratsOkTMP;
    [SerializeField] private TextMeshProUGUI congratsOkCancelTMP;

    public Action currentAction, closeAction;

    #region CONFIRMATION

    public void ShowConfirmation(string statusText, Action currentConfirmationAction, Action closeConfirmationAction)
    {
        confirmationTMP.text = statusText;
        confirmationPanelObj.SetActive(true);
        currentAction = currentConfirmationAction;
        closeAction = closeConfirmationAction;
    }

    public void ConfirmedAction()
    {

        confirmationPanelObj.SetActive(false);
        confirmationTMP.text = "";

        currentAction?.Invoke();
    }

    public void CloseConfirmationAction()
    {
        confirmationPanelObj.SetActive(false);
        confirmationTMP.text = "";

        closeAction?.Invoke();
    }

    #endregion

    #region ERROR

    public void ShowError(string statusText, Action closeConfirmationAction)
    {
        errorTMP.text = statusText;
        errorPanelObj.SetActive(true);
        closeAction = closeConfirmationAction;
    }

    public void CloseErrorAction()
    {

        errorPanelObj.SetActive(false);
        errorTMP.text = "";

        closeAction?.Invoke();
    }


    #endregion

    #region CONGRATS OK

    public void ShowCongratsOk(string statusText, Action closeConfirmationAction)
    {
        congratsOkTMP.text = statusText;
        congratsOkObj.SetActive(true);
        closeAction = closeConfirmationAction;
    }

    public void CloseCongratsOkAction()
    {
        congratsOkObj.SetActive(false);
        congratsOkTMP.text = "";

        closeAction?.Invoke();
    }

    #endregion

    #region CONGRATS OK CANCEL

    public void ShowCongratsOkCancel(string statusText, Action currentConfirmationAction, Action closeConfirmationAction)
    {
        congratsOkCancelTMP.text = statusText;
        congratsOkCancelObj.SetActive(true);
        currentAction = currentConfirmationAction;
        closeAction = closeConfirmationAction;
    }

    public void CongratsOkCancelConfirmedAction()
    {

        congratsOkCancelObj.SetActive(false);
        congratsOkCancelTMP.text = "";

        currentAction?.Invoke();
    }

    public void CloseCongratsOkCancelAction()
    {
        congratsOkCancelObj.SetActive(false);
        congratsOkCancelTMP.text = "";

        closeAction?.Invoke();
    }

    #endregion
}
