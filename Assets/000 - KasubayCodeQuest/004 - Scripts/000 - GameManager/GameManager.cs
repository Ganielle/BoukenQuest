using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //  ============================

    [SerializeField] private string startScene;

    [Header("DONT DESTROY ON LOAD")]
    [SerializeField] private List<GameObject> dontDestroyOnLoadList;

    [field: Header("CAMERAS")]
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public Camera UICamera { get; private set; }

    [field: Header("SCRIPT REFERENCES")]
    [field: SerializeField] public SceneController SceneController { get; private set; }
    [field: SerializeField] public NotificationController NotificationController { get; private set; }
    [field: SerializeField] public SoundManager SoundManager { get; private set; }


    private void Awake()
    {
        Application.targetFrameRate = 60;

        for (int a = 0; a < dontDestroyOnLoadList.Count; a++)
            DontDestroyOnLoad(dontDestroyOnLoadList[a]);

        Instance = this;

        SceneController.CurrentScene = startScene;
    }

    public Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}

