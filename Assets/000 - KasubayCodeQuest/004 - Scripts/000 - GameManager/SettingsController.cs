using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private Slider cameraSlider;

    private void OnEnable()
    {
        cameraSlider.value = userData.CameraSensitivity;
    }

    public void ChangeSensitivity()
    {
        userData.CameraSensitivity = cameraSlider.value;
    }
}
