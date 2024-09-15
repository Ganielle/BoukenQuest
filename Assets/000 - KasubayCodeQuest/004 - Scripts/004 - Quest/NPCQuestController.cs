using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuestController : MonoBehaviour
{
    [SerializeField] private QuestController questController;
    [SerializeField] private GameObject questItem;

    [Space]
    [SerializeField] private GameObject questIndicator;
    [SerializeField] private List<string> QuestList;

    private void OnEnable()
    {
        questController.OnQuestIndexChange += QuestChange;
    }

    private void OnDisable()
    {
        questController.OnQuestIndexChange -= QuestChange;
    }

    private void Start()
    {
        CheckIndicator();
    }

    private void QuestChange(object sender, EventArgs e)
    {
        CheckIndicator();
    }

    private void CheckIndicator()
    {
        if (QuestList.Contains(questController.QuestList[questController.QuestIndex].QuestID))
        {
            questIndicator.SetActive(true);

            if (questItem != null)
                questItem.SetActive(true);
        }
        else
        {
            questIndicator.SetActive(false);

            if (questItem != null)
                questItem.SetActive(false);
        }
    }

    public bool CheckIfThisIsQuest() => QuestList.Contains(questController.QuestList[questController.QuestIndex].QuestID);

    public void ProgressQuest()
    {
        if (!QuestList.Contains(questController.QuestList[questController.QuestIndex].QuestID)) return;

        questController.QuestIndex++;
    }
}
