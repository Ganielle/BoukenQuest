using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StageOneQuest : MonoBehaviour
{
    private event EventHandler ItemQuestDataChanged;
    public event EventHandler OnItemQuestDataChanged
    {
        add
        {
            if (ItemQuestDataChanged == null || !ItemQuestDataChanged.GetInvocationList().Contains(value))
                ItemQuestDataChanged += value;
        }
        remove { ItemQuestDataChanged -= value; }
    }
    public List<ItemQuestData> ItemQuestDatas
    {
        get => itemQuestDatas;
    }
    public void AddItemQuestDatas(ItemQuestData data)
    {
        if (ItemQuestDatas.Contains(data)) return;

        itemQuestDatas.Add(data);
        ItemQuestDataChanged?.Invoke(this, EventArgs.Empty);
    }

    //  =============================

    [SerializeField] private StageOneController controller;

    [Space]
    [SerializeField] private TextMeshProUGUI itemCountTMP;
    [SerializeField] private TextMeshProUGUI questStatusTMP;
    [SerializeField] private GameObject finalQuestObj;
    [SerializeField] private GameObject teleporterObj;

    [Space]
    [SerializeField] private List<ItemQuestData> questItemsSetOne;
    [SerializeField] private List<ItemQuestData> questItemsSetTwo;
    [SerializeField] private List<ItemQuestData> questItemsSetThree;
    [SerializeField] private List<ItemQuestData> questItemsSetFour;

    [Space]
    [SerializeField] private GameObject arrangementQuestObj;
    [SerializeField] private GameObject gameplayObj;
    [SerializeField] private List<HiraganaArrangeItem> hiraganaQuestItems;

    [Space]
    [SerializeField] private List<StageOneQuestItem> items;

    [Header("DEBUGGER")]
    [SerializeField] private List<ItemQuestData> itemQuestDatas;

    private void OnEnable()
    {
        CheckItemCounts();

        StartCoroutine(SetItems());

        ItemQuestDataChanged += ItemAdd;
    }

    private void OnDisable()
    {
        ItemQuestDataChanged -= ItemAdd;
    }

    private void ItemAdd(object sender, EventArgs e)
    {
        CheckItemCounts();
    }

    private void CheckItemCounts()
    {
        itemCountTMP.text = $"Hiragana Found: \n{ItemQuestDatas.Count} / 10";

        if (itemQuestDatas.Count < 10)
        {
            questStatusTMP.text = "Complete the quest first";
            finalQuestObj.SetActive(false);
            teleporterObj.SetActive(false);
        }
        else
        {
            questStatusTMP.text = "Arrange the Hiragana from 1 - 10";
            finalQuestObj.SetActive(true);
        }
    }

    public IEnumerator SetItems()
    {
        List<ItemQuestData> tempDatas = new List<ItemQuestData>();

        int rand = UnityEngine.Random.Range(0, 4);

        switch (rand)
        {
            case 0:
                tempDatas = questItemsSetOne;
                break;
            case 1:
                tempDatas = questItemsSetTwo;
                break;
            case 2:
                tempDatas = questItemsSetThree;
                break;
            case 3:
                tempDatas = questItemsSetFour;
                break;
            default:
                tempDatas = questItemsSetOne;
                break;
        }

        yield return StartCoroutine(tempDatas.Shuffle());

        for (int a = 0; a < items.Count; a++)
        {
            items[a].data = tempDatas[a];
            yield return null;
        }
    }

    public void CheckAnswer()
    {
        int correctAnswers = 0;

        for (int a = 0; a < hiraganaQuestItems.Count; a++)
        {
            if (hiraganaQuestItems[a].itemIndex.ToString() == hiraganaQuestItems[a].itemData.ItemIndex)
                correctAnswers++;
        }

        if (correctAnswers < 10)
        {
            controller.GameOver();
        }
        else
        {
            arrangementQuestObj.SetActive(false);
            gameplayObj.SetActive(true);
            teleporterObj.SetActive(true);
        }
    }
}

public static class Shuffler
{
    private static System.Random rng = new System.Random();

    public static IEnumerator Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;

            yield return null;
        }
    }
}