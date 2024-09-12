using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Mathscape/User/UserData", order = 1)]
public class UserData : ScriptableObject
{
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
    [SerializeField] private float currentHealth;

    [Space]
    [SerializeField] private float currentCameraSensitivity;

    private void OnEnable()
    {
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
}
