using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private ItemData itemData;

    public void Buy()
    {
        if (userData.CurrentMoney < itemData.ItemPrice)
        {
            GameManager.Instance.NotificationController.ShowError("You don't have enough money to buy this item.", null);
            return;
        }

        GameManager.Instance.NotificationController.ShowConfirmation($"Are you sure you want to buy this item {itemData.ItemName} for {itemData.ItemPrice}?", () =>
        {
            userData.CurrentMoney -= itemData.ItemPrice;

            if (userData.InventoryItems.ContainsKey(itemData))
                userData.InventoryItems[itemData] += 1;
            else
                userData.InventoryItems.Add(itemData, 1);

            Debug.Log($"user inventory count: {userData.InventoryItems.Count}");

            GameManager.Instance.NotificationController.ShowError("Thank you for your purchase!.", null);

        }, null);
    }
}
