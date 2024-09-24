using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultipleChoice 1", menuName = "BoukenQuest/StageThree/MultipleChoiceData")]
public class Stage3QuestionData : ScriptableObject
{
    [field: TextArea] [field: SerializeField] public string Question { get; private set; }
    [field: SerializeField] public List<string> Answers { get; private set; }
    [field: SerializeField] public string RightAnswer { get; private set; }
}
