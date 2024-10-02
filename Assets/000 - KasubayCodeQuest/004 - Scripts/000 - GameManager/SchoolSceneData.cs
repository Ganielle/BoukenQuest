using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SchoolSceneData", menuName = "BoukenQuest/Schoole/SceneData")]
public class SchoolSceneData : ScriptableObject
{
    [field: SerializeField] public int QuestIndex { get; set; }
    [field: SerializeField] public Vector3 PlayerOldPosition { get; set; }
    [field: SerializeField] public Vector3 PlayerOldRotation { get; set; }

    private void OnEnable()
    {
        ResetQuestData();
    }

    public void ResetQuestData()
    {
        QuestIndex = 0;
        PlayerOldPosition = new Vector3(-31.99f, -0.487f, -23.47f);
        PlayerOldRotation = new Vector3(0f, 0f, 0f);
    }

    public void LoadData(int questIndex, Vector3 position, Vector3 rotation)
    {
        QuestIndex = questIndex;
        PlayerOldPosition = position;
        PlayerOldRotation = rotation;
    }
}
