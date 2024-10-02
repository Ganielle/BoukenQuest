using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadGameItem : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private SchoolSceneData schoolSceneData;
    [SerializeField] private string saveid;
    [SerializeField] private GameObject loadGame;
    [SerializeField] private GameObject noLoadGame;

    [Space]
    [SerializeField] private TextMeshProUGUI usernameTMP;
    [SerializeField] private TextMeshProUGUI timeAndDateTMP;

    [Header("FOR SAVING")]
    [SerializeField] private Transform player;

    [Header("DEBUGGER")]
    [SerializeField] private string username;
    [SerializeField] private string datetime;

    private void OnEnable()
    {
        CheckData();
    }

    private void CheckData()
    {
        if (PlayerPrefs.HasKey(saveid))
        {
            Dictionary<string, object> loaddata = JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString(saveid));

            usernameTMP.text = loaddata["username"].ToString();
            timeAndDateTMP.text = loaddata["dateandtime"].ToString();

            username = loaddata["username"].ToString();
            datetime = loaddata["dateandtime"].ToString();

            loadGame.SetActive(true);
            noLoadGame.SetActive(false);
        }
        else
        {
            loadGame.SetActive(false);
            noLoadGame.SetActive(true);
        }
    }

    public void LoadGame()
    {
        GameManager.Instance.NotificationController.ShowConfirmation($"Are you sure you want to load this save data ({username}, {datetime})", () =>
        {
            Dictionary<string, object> loaddata = JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString(saveid));

            Dictionary<ItemData, int> inventoryitems = JsonConvert.DeserializeObject<Dictionary<ItemData, int>>(loaddata["inventory"].ToString());
            Dictionary<string, int> playerstatistics = JsonConvert.DeserializeObject<Dictionary<string, int>>(loaddata["playerstats"].ToString());

            schoolSceneData.LoadData(Convert.ToInt32(loaddata["index"].ToString()), new Vector3(float.Parse(loaddata["positionx"].ToString()), float.Parse(loaddata["positiony"].ToString()), float.Parse(loaddata["positionz"].ToString())), new Vector3(float.Parse(loaddata["rotationx"].ToString()), float.Parse(loaddata["rotationy"].ToString()), float.Parse(loaddata["rotationz"].ToString())));
            userData.LoadGameData(float.Parse(loaddata["health"].ToString()), userData.CurrentUsername, float.Parse(loaddata["money"].ToString()), inventoryitems, playerstatistics);

            GameManager.Instance.SceneController.CurrentScene = "School";
        }, null);
    }

    public void SaveGame()
    {
        if (PlayerPrefs.HasKey(saveid))
        {
            GameManager.Instance.NotificationController.ShowConfirmation($"There's an existing data! Are you sure you want to rewrite this data?", () =>
            {
                Dictionary<string, object> savedata = new()
                {
                    { "username", userData.CurrentUsername },
                    { "dateandtime", System.DateTime.Now.ToString("yyyy / MM / dd HH: mm:ss") },
                    { "index", schoolSceneData.QuestIndex },
                    { "positionx", player.position.x },
                    { "positiony", player.position.y },
                    { "positionz", player.position.z },
                    { "rotationx", player.rotation.x },
                    { "rotationy", player.rotation.y },
                    { "rotationz", player.rotation.z },
                    { "health", userData.CurrentHealth },
                    { "inventory", JsonConvert.SerializeObject(userData.InventoryItems) },
                    { "money", userData.CurrentMoney},
                    { "playerstats", JsonConvert.SerializeObject(userData.PlayerStatistics) }
                };

                string finalsavedata = JsonConvert.SerializeObject(savedata);

                PlayerPrefs.SetString(saveid, finalsavedata);

                CheckData();
            }, null);
        }
        else
        {
            GameManager.Instance.NotificationController.ShowConfirmation($"Are you sure you want to save your progress?", () =>
            {
                Dictionary<string, object> savedata = new()
                {
                    { "username", userData.CurrentUsername },
                    { "dateandtime", System.DateTime.Now.ToString("yyyy / MM / dd HH: mm:ss") },
                    { "index", schoolSceneData.QuestIndex },
                    { "positionx", player.position.x },
                    { "positiony", player.position.y },
                    { "positionz", player.position.z },
                    { "rotationx", player.rotation.x },
                    { "rotationy", player.rotation.y },
                    { "rotationz", player.rotation.z },
                    { "health", userData.CurrentHealth },
                    { "inventory", JsonConvert.SerializeObject(userData.InventoryItems) },
                    { "money", userData.CurrentMoney },
                    { "playerstats", JsonConvert.SerializeObject(userData.PlayerStatistics) }
                };

                string finalsavedata = JsonConvert.SerializeObject(savedata);

                PlayerPrefs.SetString(saveid, finalsavedata);

                CheckData();
            }, null);
        }
    }

    public void DeleteGame()
    {
        GameManager.Instance.NotificationController.ShowConfirmation($"Are you sure you want to delete this save data ({username}, {datetime})", () =>
        {
            PlayerPrefs.DeleteKey(saveid);
            CheckData();
        }, null);
    }
}
