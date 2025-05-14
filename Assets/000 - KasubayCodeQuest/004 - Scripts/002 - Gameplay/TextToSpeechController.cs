using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToSpeechController : MonoBehaviour
{

    [SerializeField] private AudioSource speechSource;

    private void Update()
    {
        speechSource.volume = GameManager.Instance.SoundManager.CurrentSFXVolume;

        if (speechSource.isPlaying)
            GameManager.Instance.SoundManager.SetBGMusicToLow();
        else
            GameManager.Instance.SoundManager.ResetBGMusicVolume();
    }

    public void PlaySpeech(AudioClip clip)
    {
        if (speechSource.isPlaying) speechSource.Stop();

        speechSource.clip = clip;
        speechSource.Play();
    }
}
