using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStageOneHiragana : MonoBehaviour
{
    [SerializeField] private StageOneQuest stageOneQuest;
    [SerializeField] private GameObject arrangementQuest;
    [SerializeField] private GameObject gameplayObj;
    [SerializeField] private List<HiraganaArrangeItem> hiraganaQuestItems;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinalQuest"))
        {
            for (int a = 0; a < stageOneQuest.ItemQuestDatas.Count; a++)
            {
                hiraganaQuestItems[a].SetData(stageOneQuest.ItemQuestDatas[a]);
            }

            arrangementQuest.SetActive(true);
            gameplayObj.SetActive(false);
        }
    }
}
