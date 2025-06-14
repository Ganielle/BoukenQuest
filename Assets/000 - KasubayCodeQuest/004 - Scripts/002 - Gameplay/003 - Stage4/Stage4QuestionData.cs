using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultipleChoice 1", menuName = "BoukenQuest/StageFour/MultipleChoiceData")]
public class Stage4QuestionData : ScriptableObject
{
    [field: TextArea] [field: SerializeField] public string Question { get; private set; }
    [field: SerializeField] public List<string> Answers { get; private set; }
    [field: SerializeField] public string RightAnswer { get; private set; }

    [field: Space]
    [field: SerializeField] public AudioClip QuestionClip { get; private set; }
    [field: SerializeField] public List<AudioClip> AnswerClip { get; private set; }
}
