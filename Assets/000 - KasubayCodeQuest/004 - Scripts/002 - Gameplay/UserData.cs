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

    [Header("DEBUGGER")]
    [SerializeField] private string username;
    [SerializeField] private float currentHealth;

    [Space]
    [SerializeField] private float currentCameraSensitivity;

    private void OnEnable()
    {
        CurrentUsername = "";
        CurrentHealth = 100f;
        CheckCameraSensitivity();
    }

    private void CheckCameraSensitivity()
    {
        if (PlayerPrefs.HasKey("CameraSensitivity"))
            currentCameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity");
        else
            CameraSensitivity = 0.2f;
    }

    public void LoadGameData(float health, string name)
    {
        CurrentUsername = name;
        CurrentHealth = health;
    }
}
