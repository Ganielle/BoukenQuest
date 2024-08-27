using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HiraganaArrangeItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hiraganaTMP;
    [SerializeField] private TextMeshProUGUI indexTMP;

    [Header("DEBUGGER")]
    [SerializeField] public ItemQuestData itemData;
    [SerializeField] public int itemIndex;

    public void SetData(ItemQuestData item)
    {
        itemData = item;
        hiraganaTMP.text = $"{item.ItemCharacters} ({item.EnglishTranslation})";
        itemIndex = 0;
        indexTMP.text = "0";
    }

    public void PreviousIndex()
    {
        if (itemIndex <= 0)
            itemIndex = 10;
        else
            itemIndex--;

        indexTMP.text = $"{itemIndex}";
    }
    
    public void NextIndex()
    {
        if (itemIndex >= 10)
            itemIndex = 0;
        else
            itemIndex++;

        indexTMP.text = $"{itemIndex}";
    }
}
