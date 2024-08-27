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

    [Header("DEBUGGER")]
    [SerializeField] private float currentHealth;

    private void OnEnable()
    {
        CurrentHealth = 100f;
    }
}
