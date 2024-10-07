using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Mathscape/User/UserData", order = 1)]
public class UserData : ScriptableObject
{
    public string CurrentUsername
    {
        get => username;
        set => username = value;
    }

    private event EventHandler HealthChange;
    public event EventHandler OnHealhChange
    {
        add
        {
            if (HealthChange == null || !HealthChange.GetInvocationList().Contains(value))
                HealthChange += value;
        }
        remove { HealthChange -= value; }
    }

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            HealthChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public float CameraSensitivity
    {
        get => currentCameraSensitivity;
        set
        {
            currentCameraSensitivity = value;
            PlayerPrefs.SetFloat("CameraSensitivity", currentCameraSensitivity);
        }
    }

    private event EventHandler MoneyChange;
    public event EventHandler OnMoneyChange
    {
        add
        {
            if (MoneyChange == null || !MoneyChange.GetInvocationList().Contains(value)) MoneyChange += value;
        }
        remove { MoneyChange -= value; }
    }
    public float CurrentMoney
    {
        get => currentMoney;
        set
        {
            currentMoney = value;
            MoneyChange?.Invoke(this, EventArgs.Empty);
        }
    }

    [Header("DEBUGGER")]
    [SerializeField] private string username;
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentMoney;

    [Space]
    [SerializeField] private float currentCameraSensitivity;

    //  ===================

    public Dictionary<string, int> PlayerStatistics { get; private set; }
    public Dictionary<ItemData, int> InventoryItems { get; private set; }

    //  ===================

    private void OnEnable()
    {
        InventoryItems = new Dictionary<ItemData, int>();
        PlayerStatistics = new Dictionary<string, int>();
        CurrentUsername = "";
        CurrentHealth = 100f;
        CurrentMoney = 0f;
        CheckCameraSensitivity();
    }

    public void SetPlayerStatistics()
    {
        PlayerStatistics.Clear();

        PlayerStatistics.Add("stageOneRightAnswers", 0);
        PlayerStatistics.Add("stageOneWrongAnswers", 0);
        PlayerStatistics.Add("stageTwoRightAnswers", 0);
        PlayerStatistics.Add("stageTwoWrongAnswers", 0);
        PlayerStatistics.Add("stageThreeRightAnswers", 0);
        PlayerStatistics.Add("stageThreeWrongAnswers", 0);
        PlayerStatistics.Add("stageFourRightAnswers", 0);
        PlayerStatistics.Add("stageFourWrongAnswers", 0);
        PlayerStatistics.Add("stageFiveRightAnswers", 0);
        PlayerStatistics.Add("stageFiveWrongAnswers", 0);
    }

    private void CheckCameraSensitivity()
    {
        if (PlayerPrefs.HasKey("CameraSensitivity"))
            currentCameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity");
        else
            CameraSensitivity = 0.2f;
    }

    public void LoadGameData(float health, string name, float money, Dictionary<ItemData, int> inventory, Dictionary<string, int> playerStatistics)
    {
        PlayerStatistics.Clear();
        CurrentUsername = name;
        CurrentHealth = health;
        CurrentMoney = money;
        InventoryItems = inventory;
        PlayerStatistics = playerStatistics;
    }
}
