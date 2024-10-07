using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private SchoolSceneData schoolSceneData;
    [SerializeField] private UserData userData;
    [SerializeField] private AudioClip backgroundClip;

    [Header("ENTER YOUR NAME")]
    [SerializeField] private TMP_InputField usernameTMP;
    [SerializeField] private GameObject nextBtn;

    private void OnEnable()
    {
        GameManager.Instance.SoundManager.SetBGMusic(backgroundClip);
        GameManager.Instance.SceneController.ActionPass = true;
    }

    public void CheckUsername()
    {
        if (usernameTMP.text == "") nextBtn.SetActive(false);
        else nextBtn.SetActive(true);
    }

    public void EnterUsernameToMainMenu()
    {
        usernameTMP.text = "";
        nextBtn.SetActive(false);
    }

    public void StartGame()
    {
        userData.CurrentUsername = usernameTMP.text;
        userData.SetPlayerStatistics();
        schoolSceneData.ResetQuestData();
        GameManager.Instance.SceneController.CurrentScene = "School";
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
