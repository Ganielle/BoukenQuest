using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SentencesData", menuName = "BoukenQuest/StageTwo/SentencesData")]
public class SentencesData : ScriptableObject
{
    [field: SerializeField] public List<Sentences> SentencesList { get; set; }
}

[System.Serializable]
public struct Sentences
{
    public string characters;
    public string translation;
    public string index;
    public AudioClip textToSpeech;
}
