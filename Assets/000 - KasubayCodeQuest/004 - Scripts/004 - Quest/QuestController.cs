using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private List<QuestData> questDatas;

    [Header("DEBUGGER")]
    [SerializeField] private int questIndex;
}
