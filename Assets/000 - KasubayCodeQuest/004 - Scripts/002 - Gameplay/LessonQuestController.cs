using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonQuestController : MonoBehaviour
{
    [SerializeField] private QuestController questController;
    [SerializeField] private int questIndex;
    [SerializeField] private Collider colliderItem;

    private void OnEnable()
    {
        Check();
        questController.OnQuestIndexChange += QuestChange;
    }

    private void OnDisable()
    {
        questController.OnQuestIndexChange -= QuestChange;
    }

    private void QuestChange(object sender, EventArgs e)
    {
        Check();
    }

    private void Check()
    {
        if (questController.QuestIndex < questIndex)
            colliderItem.enabled = false;
        else
            colliderItem.enabled = true;
    }
}
