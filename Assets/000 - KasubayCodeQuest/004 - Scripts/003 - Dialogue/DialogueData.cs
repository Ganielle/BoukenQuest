using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Mathscape/Dialogue/DialogueData", order = 1)]
public class DialogueData : ScriptableObject
{
    [Header("CONTENT")]
    [field: SerializeField] public List<string> nameSequence { get; private set; }
    [field: TextArea] [field: SerializeField] public List<string> dialogueSequence { get; private set; }
}