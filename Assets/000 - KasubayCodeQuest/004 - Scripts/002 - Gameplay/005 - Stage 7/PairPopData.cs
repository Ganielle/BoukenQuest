using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Mathscape/Stage7/PairPopData", order = 1)]
public class PairPopData : ScriptableObject
{
    [field: SerializeField] public string pairOne;
    [field: SerializeField] public string pairTwo;
    [field: SerializeField] public AudioClip pairPopOne;
    [field: SerializeField] public AudioClip pairPopTwo;
}
