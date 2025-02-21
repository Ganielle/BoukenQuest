using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            StartCoroutine(Load());
        }, null);
    }

    IEnumerator Load()
    {
        Dictionary<string, object> loaddata = JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString(saveid));

        Dictionary<string, int> tempInventory = JsonConvert.DeserializeObject<Dictionary<string, int>>(loaddata["inventory"].ToString());

        Dictionary<ItemData, int> playertempinventory = new Dictionary<ItemData, int>();

        Dictionary<string, int> playerstatistics = JsonConvert.DeserializeObject<Dictionary<string, int>>(loaddata["playerstats"].ToString());

        foreach (var key in tempInventory)
        {
            Debug.Log(key.Key);
            foreach (ItemData item in GameManager.Instance.ItemList)
            {
                Debug.Log(key.Key == item.ItemID);
                if (key.Key == item.ItemID)
                {
                    if (playertempinventory.ContainsKey(item))
                        playertempinventory[item] += key.Value;
                    else
                        playertempinventory.Add(item, key.Value);
                }

                yield return null;
            }

            yield return null;
        }

        schoolSceneData.LoadData(Convert.ToInt32(loaddata["index"].ToString()), new Vector3(float.Parse(loaddata["positionx"].ToString()), float.Parse(loaddata["positiony"].ToString()), float.Parse(loaddata["positionz"].ToString())), new Vector3(float.Parse(loaddata["rotationx"].ToString()), float.Parse(loaddata["rotationy"].ToString()), float.Parse(loaddata["rotationz"].ToString())));
        userData.LoadGameData(float.Parse(loaddata["health"].ToString()), loaddata["username"].ToString(), float.Parse(loaddata["money"].ToString()), playertempinventory, playerstatistics);

        GameManager.Instance.SceneController.CurrentScene = "School";
    }

    public void SaveGame()
    {
        if (PlayerPrefs.HasKey(saveid))
        {
            GameManager.Instance.NotificationController.ShowConfirmation($"There's an existing data! Are you sure you want to rewrite this data?", () =>
            {
                Dictionary<string, int> tempInventory = userData.InventoryItems.ToDictionary(item => item.Key.ItemID, item => item.Value);

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
                    { "inventory", JsonConvert.SerializeObject(tempInventory) },
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
                Dictionary<string, int> tempInventory = userData.InventoryItems.ToDictionary(item => item.Key.ItemID, item => item.Value);

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
                    { "inventory", JsonConvert.SerializeObject(tempInventory) },
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
