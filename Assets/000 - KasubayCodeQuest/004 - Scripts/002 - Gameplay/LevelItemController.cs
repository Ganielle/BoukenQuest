using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelItemController : MonoBehaviour
{
    [SerializeField] private QuestController questController;
    [SerializeField] private int startQuestLevelIndex;
    [SerializeField] private GameObject lockObj;
    [SerializeField] private GameObject openObj;

    private void OnEnable()
    {
        if (questController.QuestIndex < startQuestLevelIndex)
        {
            lockObj.SetActive(true);
            openObj.SetActive(false);
        }
        else
        {
            lockObj.SetActive(false);
            openObj.SetActive(true);
        }
    }
}
