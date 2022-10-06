using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioListener listener;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip titleScreenClip;
    [SerializeField] private AudioClip dockStationClip;
    [SerializeField] private AudioClip[] stageClips;
    [SerializeField] private AudioClip addCoins;
    [SerializeField] private AudioClip addCow;
    public float MusicVolume { get; set; }
    public float SFXVolume { get; set; }

    public void EnableMainMenuMusic()
    {
        EnableAudioListener();
    }

    public void PlayIntroMusic()
    {
        EnableMainMenuMusic();
        music.clip = titleScreenClip;
        music.Play();
    }

    public void PlayDockStationMusic()
    {
        EnableMainMenuMusic();
        music.clip = dockStationClip;
        music.Play();
    }


    public void PlayStageMusic()
    {
        DisableAudioListener();
        if(stageClips.Length > 0)
        {
            int musicIndex = Random.Range(1, stageClips.Length);
            music.clip = stageClips[musicIndex];
            music.Play();
        }
        
    }


    private void DisableAudioListener()
    {
        listener.enabled = false;
    }

    private void EnableAudioListener()
    {
        listener.enabled = true;
    }
}
