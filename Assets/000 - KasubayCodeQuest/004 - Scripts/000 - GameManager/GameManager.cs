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

    [field: Header("SCRIPT REFERENCES")]
    [field: SerializeField] public SceneController SceneController { get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        for (int a = 0; a < dontDestroyOnLoadList.Count; a++)
            DontDestroyOnLoad(dontDestroyOnLoadList[a]);

        Instance = this;

        SceneController.CurrentScene = startScene;
    }
}
