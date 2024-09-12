using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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

    [SerializeField] private TextMeshProUGUI questTMP;
    [SerializeField] private List<QuestData> questDatas;

    [Header("DEBUGGER")]
    [SerializeField] private int questIndex;

    private void OnEnable()
    {
        questTMP.text = questDatas[questIndex].QuestName;
        OnQuestIndexChange += QuestChange;
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void OnDisable()
    {
        OnQuestIndexChange -= QuestChange;
    }

    private void QuestChange(object sender, EventArgs e)
    {
        questTMP.text = questDatas[questIndex].QuestName;
    }

    public void NextQuest(int tempQuestIndex)
    {
        if (tempQuestIndex == questIndex) return;

        QuestIndex++;
    }

    public void ChangeScene(string sceneName) => GameManager.Instance.SceneController.CurrentScene = sceneName;

    public void QuitGame()
    {
        GameManager.Instance.SceneController.CurrentScene = "MainMenu";
    }
}
