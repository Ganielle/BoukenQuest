using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private UserData userData;
    [SerializeField] private Slider cameraSlider;
    [SerializeField] private Slider bgSlider;
    [SerializeField] private Slider sfxSlider;

    private void OnEnable()
    {
        bgSlider.value = GameManager.Instance.SoundManager.CurrenBGVolume;
        sfxSlider.value = GameManager.Instance.SoundManager.CurrentSFXVolume;
        cameraSlider.value = userData.CameraSensitivity;
    }

    public void ChangeSensitivity()
    {
        userData.CameraSensitivity = cameraSlider.value;
    }

    public void ChangeSFX()
    {
        GameManager.Instance.SoundManager.CurrentSFXVolume = sfxSlider.value;
    }

    public void ChangeBG()
    {
        GameManager.Instance.SoundManager.CurrenBGVolume = bgSlider.value;
    }
}
