using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private event EventHandler VolumeChange;
    public event EventHandler OnVolumeChange
    {
        add
        {
            if (VolumeChange == null || !VolumeChange.GetInvocationList().Contains(value))
                VolumeChange += value;
        }
        remove { VolumeChange -= value; }
    }
    public float CurrenBGVolume
    {
        get => currentBGVolume;
        set
        {
            currentBGVolume = value;
            VolumeChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler SFXChange;
    public event EventHandler OnSFXChange
    {
        add
        {
            if (SFXChange == null || !SFXChange.GetInvocationList().Contains(value))
                SFXChange += value;
        }
        remove { SFXChange -= value; }
    }
    public float CurrentSFXVolume
    {
        get => currentSFXVolume;
        set
        {
            currentSFXVolume = value;
            SFXChange?.Invoke(this, EventArgs.Empty);
        }
    }

    [SerializeField] private AudioSource bgSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("DEBUGGER")]
    [SerializeField] private float currentBGVolume;
    [SerializeField] private float currentSFXVolume;

    private void Awake()
    {
        CheckBGVolumeSaveData();

        OnVolumeChange += VolumeBGCheck;
        OnSFXChange += VolumeSFXCheck;
    }

    private void OnDisable()
    {
        OnVolumeChange -= VolumeBGCheck;
        OnSFXChange += VolumeSFXCheck;
    }

    private void VolumeBGCheck(object sender, EventArgs e)
    {
        ChangeVolume();
    }

    private void VolumeSFXCheck(object sender, EventArgs e)
    {
        ChangeVolume();
    }

    public void CheckBGVolumeSaveData()
    {
        if (!PlayerPrefs.HasKey("bgVolumeData"))
        {
            CurrenBGVolume = 1;
            PlayerPrefs.SetFloat("bgVolumeData", CurrenBGVolume);
        }
        else
            CurrenBGVolume = PlayerPrefs.GetFloat("bgVolumeData");


        if (!PlayerPrefs.HasKey("sfxVolumeData"))
        {
            CurrentSFXVolume = 1;
            PlayerPrefs.SetFloat("sfxVolumeData", CurrentSFXVolume);
        }
        else
            CurrentSFXVolume = PlayerPrefs.GetFloat("sfxVolumeData");
    }

    public void SetBGMusic(AudioClip clip)
    {
        LeanTween.value(gameObject, CurrenBGVolume, 0f, 0.25f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
        {
            bgSource.volume = val;
        }).setOnComplete(() => 
        {
            bgSource.Stop();
            bgSource.clip = clip;
            bgSource.Play();
            LeanTween.value(gameObject, 0f, CurrenBGVolume, 0.25f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) => 
            {
                bgSource.volume = val;
            }).setOnComplete(() => 
            {
                bgSource.volume = CurrenBGVolume;
            });
        });
    }

    public void PlaySFX(AudioClip clip) => sfxSource.PlayOneShot(clip);

    private void ChangeVolume()
    {
        bgSource.volume = CurrenBGVolume;
        sfxSource.volume = CurrentSFXVolume;
        PlayerPrefs.SetFloat("bgVolumeData", CurrenBGVolume);
        PlayerPrefs.SetFloat("sfxVolumeData", CurrentSFXVolume);
    }
}
