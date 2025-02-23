using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{
    private event EventHandler QuestIndexChange;
    public event EventHandler OnQuestIndexChange
    {
        add
        {
            if (QuestIndexChange == null || !QuestIndexChange.GetInvocationList().Contains(value))
                QuestIndexChange += value;
        }
        remove
        {
            QuestIndexChange -= value;
        }
    }
    public int QuestIndex
    {
        get => questIndex;
        set
        {
            questIndex = value;
            QuestIndexChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<QuestData> QuestList
    {
        get => questDatas;
    }

    //  =======================

    [SerializeField] private UserData userData;
    [SerializeField] private SchoolSceneData schoolSceneData;
    [SerializeField] private TextMeshProUGUI questTMP;
    [SerializeField] private Transform player;
    [SerializeField] private List<QuestData> questDatas;
    [SerializeField] private Slider healthSlider;

    [Space]
    [SerializeField] private AudioClip backgroundMusicClip;

    [Space]
    [SerializeField] private TextMeshProUGUI playerStatsTMP;
    [SerializeField] private TextMeshProUGUI endPlayerStatsTMP;

    [Header("DEBUGGER")]
    [SerializeField] private int questIndex;

    private void OnEnable()
    {
        GameManager.Instance.SoundManager.SetBGMusic(backgroundMusicClip);
        OnQuestIndexChange += QuestChange;
        QuestIndex = schoolSceneData.QuestIndex;
        questTMP.text = questDatas[questIndex].QuestName;
        player.transform.position = schoolSceneData.PlayerOldPosition;
        player.transform.rotation = Quaternion.Euler(schoolSceneData.PlayerOldRotation);

        healthSlider.value = userData.CurrentHealth / 100;

        userData.OnHealhChange += HealthChange;

        if (userData.PlayerStatistics.Count > 0)
        {
            playerStatsTMP.text = $"Stage 1:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageOneRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageOneWrongAnswers"]}\n\n" +
                $"Stage 2:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageTwoRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageTwoWrongAnswers"]}\n\n" +
                $"Stage 3:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageThreeRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageThreeWrongAnswers"]}\n\n" +
                $"Stage 4:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageFourRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageFourWrongAnswers"]}\n\n" +
                $"Stage 5:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageFiveRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageFiveWrongAnswers"]}\n\n" +
                $"Stage 6:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageSixRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageSixWrongAnswers"]}\n\n" +
                $"Stage 7:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageSevenRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageSevenWrongAnswers"]}\n\n"; ;


            endPlayerStatsTMP.text = $"Stage 1:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageOneRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageOneWrongAnswers"]}\n\n" +
                $"Stage 2:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageTwoRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageTwoWrongAnswers"]}\n\n" +
                $"Stage 3:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageThreeRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageThreeWrongAnswers"]}\n\n" +
                $"Stage 4:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageFourRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageFourWrongAnswers"]}\n\n" +
                $"Stage 5:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageFiveRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageFiveWrongAnswers"]}\n\n" +
                $"Stage 6:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageSixRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageSixWrongAnswers"]}\n\n" +
                $"Stage 7:\n\n" +
                $"Right Answers: {userData.PlayerStatistics["stageSevenRightAnswers"]}\n" +
                $"Wrong Answers: {userData.PlayerStatistics["stageSevenWrongAnswers"]}\n\n";
        }
        else
        {
            playerStatsTMP.text = "No data yet!\n Please complete the stages on the map first!";
            endPlayerStatsTMP.text = "No data yet!\n Please complete the stages on the map first!";
        }

        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void OnDisable()
    {
        OnQuestIndexChange -= QuestChange;
        userData.OnHealhChange -= HealthChange;
    }

    private void HealthChange(object sender, EventArgs e)
    {
        healthSlider.value = userData.CurrentHealth / 100;
    }

    private void QuestChange(object sender, EventArgs e)
    {
        questTMP.text = questDatas[questIndex].QuestName;
    }

    public void NextQuest(int tempQuestIndex)
    {
        if (tempQuestIndex == questIndex) return;

        if (questIndex > tempQuestIndex) return;

        QuestIndex++;
        schoolSceneData.QuestIndex = QuestIndex;
    }

    public void ChangeScene(string sceneName) => GameManager.Instance.SceneController.CurrentScene = sceneName;

    public void SetPlayerQuestDataTemp()
    {
        schoolSceneData.QuestIndex = QuestIndex;
        schoolSceneData.PlayerOldPosition = player.transform.position;
        schoolSceneData.PlayerOldRotation = new Vector3(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z);
    }

    public void QuitGame()
    {
        GameManager.Instance.SceneController.CurrentScene = "MainMenu";
    }
}
