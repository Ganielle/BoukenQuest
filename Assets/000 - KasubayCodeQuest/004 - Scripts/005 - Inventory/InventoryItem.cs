using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private TextMeshProUGUI qtyTMP;
    [SerializeField] private Image iconImg;
    [SerializeField] private Button useBtn;

    [Header("DEBUGGER")]
    [SerializeField] private ItemData item;

    private void OnDisable()
    {
        item = null;
    }

    public void SetData(ItemData item, string qty, Sprite icon, bool isUsable)
    {
        if (icon == null)
        {
            iconImg.gameObject.SetActive(false);
            qtyTMP.text = "";

            return;
        }

        this.item = item;
        qtyTMP.text = $"x {qty}";
        iconImg.sprite = icon;
        iconImg.gameObject.SetActive(true);

        if (!isUsable) useBtn.interactable = false;
    }

    public void UseItem()
    {
        if (item == null) return;

        GameManager.Instance.NotificationController.ShowConfirmation($"Are you sure you want to use {item.ItemName} x1?", () =>
        {
            switch (item.ItemID)
            {
                case "item-002":
                    userData.CurrentHealth += 20;

                    userData.InventoryItems[item] -= 1;

                    if (userData.InventoryItems[item] <= 0)
                    {
                        userData.InventoryItems.Remove(item);
                        item = null;
                    }

                    break;
            }

            inventory.SetActive(false);
            gameplay.SetActive(true);
        }, null);
    }
}
