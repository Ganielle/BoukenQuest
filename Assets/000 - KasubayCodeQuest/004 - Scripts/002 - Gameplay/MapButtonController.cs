using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButtonController : MonoBehaviour
{
    [SerializeField] private QuestController questController;
    [SerializeField] private int mapQuestStartIndex;
    [SerializeField] private Button mapBtn;

    private void OnEnable()
    {
        questController.OnQuestIndexChange += QuestChange;
        CheckMapQuest();
    }

    private void OnDisable()
    {
        questController.OnQuestIndexChange -= QuestChange;
    }

    private void QuestChange(object sender, EventArgs e)
    {
        CheckMapQuest();
    }

    private void CheckMapQuest()
    {
        if (questController.QuestIndex < mapQuestStartIndex)
            mapBtn.interactable = false;
        else
            mapBtn.interactable = true;
    }
}
