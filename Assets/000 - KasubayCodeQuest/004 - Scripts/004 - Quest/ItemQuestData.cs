using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemQuestData", menuName = "Mathscape/Quest/ItemQuestData")]
public class ItemQuestData : ScriptableObject
{
    [field: SerializeField] public string ItemCharacters { get; private set; }
    [field: SerializeField] public string EnglishTranslation { get; private set; }
    [field: SerializeField] public string ItemIndex { get; private set; }
}
