using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private GameObject iventoryObj;
    [SerializeField] private GameObject gameplayControllerObj;
    [SerializeField] private List<InventoryItem> inventoryItems;

    [Space]
    [SerializeField] private TextMeshProUGUI gameplayMoneyTMP;
    [SerializeField] private TextMeshProUGUI storeMoneyTMP;

    [Header("ITEM DEBUGGER DELETE THIS AFTER")]
    [SerializeField] private ItemData potionItem;

    private void OnEnable()
    {
        gameplayMoneyTMP.text = $"{userData.CurrentMoney:n2}";
        storeMoneyTMP.text = $"Current Money: {userData.CurrentMoney:n2}";

        userData.OnMoneyChange += MoneyChange;
    }

    private void OnDisable()
    {
        userData.OnMoneyChange -= MoneyChange;
    }

    private void Update()
    {
        AddHealthPotion();
    }

    private void MoneyChange(object sender, EventArgs e)
    {
        gameplayMoneyTMP.text = $"{userData.CurrentMoney:n2}";
        storeMoneyTMP.text = $"{userData.CurrentMoney:n2}";
    }

    public void SetInventoryItems()
    {
        for (int a = 0; a < inventoryItems.Count; a++)
        {
            if (a >= userData.InventoryItems.Count)
                inventoryItems[a].SetData(null, "", null, false);
            else
                inventoryItems[a].SetData(userData.InventoryItems.ElementAt(a).Key, userData.InventoryItems.ElementAt(a).Value.ToString(), userData.InventoryItems.ElementAt(a).Key.ItemSprite, userData.InventoryItems.ElementAt(a).Key.IsUsable);
        }

        iventoryObj.SetActive(true);
        gameplayControllerObj.SetActive(false);
    }

    private void AddHealthPotion()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            if (userData.InventoryItems.ContainsKey(potionItem))
            {
                if (userData.InventoryItems[potionItem] < 99)
                    userData.InventoryItems[potionItem]++;
            }
            else
            {
                userData.InventoryItems.Add(potionItem, 1);
            }
        }
    }
}
