using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOneQuestItem : MonoBehaviour
{
    [SerializeField] private StageOneQuest questController;

    [Header("DEBUGGER")]
    public ItemQuestData data;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questController.AddItemQuestDatas(data);
            gameObject.SetActive(false);
        }
    }
}
