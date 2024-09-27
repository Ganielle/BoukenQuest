using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question (1)", menuName = "Codemancer/BattleSystem/QuestionItem", order = 1)]
public class QuestionData : ScriptableObject
{
    [field: TextArea][field: SerializeField] public string Question { get; private set; }
    [field: SerializeField] public List<string> Answers { get; private set; }
    [field: SerializeField] public string RightAnswer { get; private set; }
    [field: SerializeField] public string Tips { get; private set; }
    [field: SerializeField] public bool HaveCodeQuestion { get; private set; }
    [field: TextArea] [field: SerializeField] public string CodeQuestion { get; private set; }
}
