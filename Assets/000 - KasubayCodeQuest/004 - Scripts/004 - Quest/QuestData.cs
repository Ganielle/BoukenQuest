using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Mathscape/Quest/QuestData", order = 1)]
public class QuestData : ScriptableObject
{
    [field: SerializeField] public string QuestID { get; private set; }
    [field: SerializeField] public ItemQuest Item { get; private set; }
    [field: SerializeField] public List<Dialouges> Dialouges { get; private set; }
    [field: SerializeField] public List<QuestPosition> Positions { get; private set; }
}

[Serializable]
public struct Dialouges
{
    public string characterName;
    public DialogueData dialogue;
}

[Serializable]
public struct QuestPosition
{
    public string characterName;
    public Vector3 position;
    public Vector3 rotation;
}

[Serializable]
public struct ItemQuest
{
    public string itemID;
}