using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class Audio : MonoBehaviour
    {
        [SerializeField] private AudioListener listener;
        [SerializeField] private AudioSource ambientDay;
        [SerializeField] private AudioSource ambientNight;
        [SerializeField] private AudioSource music;
        [SerializeField] private AudioSource menuFlyingSaucerNavigation;
        [SerializeField] private AudioSource menuFlyingSaucerSelect;
        [SerializeField] private AudioSource buyItem;
        [SerializeField] private AudioSource levelUpWindow;
        [SerializeField] private AudioSource openWindow;
        [SerializeField] private AudioSource closeWindow;
        [SerializeField] private AudioSource pressButton;
        [SerializeField] private AudioSource pauseGame;
        [SerializeField] private AudioSource resumeGame;
        [SerializeField] private AudioSource stageClear;
        [SerializeField] private AudioClip titleScreenClip;
        [SerializeField] private AudioClip dockStationClip;
        [SerializeField] private AudioClip[] stageClips;
        [SerializeField] private AudioSource[] audioSources;
        [SerializeField] private Transform sfxSources;

        public delegate void AudioEvent();
        public static AudioEvent MuteSFX, UnmuteSFX;

        public float MusicVolume { get; set; }
        public float SFXVolume { get; set; }

        private void Awake()
        {
            audioSources = sfxSources.GetComponentsInChildren<AudioSource>();
        }



        public void DisableSFX()
        {
            if (MuteSFX != null) MuteSFX();
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.mute = true;
            }
        }

        public void DisableMusic()
        {
            music.mute = true;

        }

        public void EnableSFX()
        {
            if (UnmuteSFX != null) UnmuteSFX();
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.mute = false;
            }
        }

        public void EnableMusic()
        {
            music.mute = false;
        }

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
            if (stageClips.Length > 0)
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

        public void PlayMenuFlyingSaucerNavigationButton()
        {
            menuFlyingSaucerNavigation.Play();
        }

        public void OpenWindow()
        {
            openWindow.Play();
        }

        public void CloseWindow()
        {
            closeWindow.Play();
        }

        public void FlyingSaucerSelect()
        {
            menuFlyingSaucerSelect.Play();
        }

        public void BuyItem()
        {
            buyItem.Play();
        }

        public void LevelUpWindow()
        {
            levelUpWindow.Play();
        }

        public void PressButton()
        {
            pressButton.Play();
        }
        public void PauseGame()
        {
            pauseGame.Play();
        }
        public void ResumeGame()
        {
            resumeGame.Play();
        }
        public void StageClear()
        {
            stageClear.Play();
            music.volume = 0f;
            ambientDay.volume = 0f;
            ambientNight.volume = 0f;
            StartCoroutine(RestoreMusicVolume());
        }

        IEnumerator RestoreMusicVolume()
        {
            yield return new WaitForSeconds(2f);
            for (float i = 0f; i <= 1.0f; i += 0.02f)
            {
                music.volume = i;
                ambientDay.volume = i;
                ambientNight.volume = i;
                yield return null;
            }
        }

        public void PlayAmbientDay()
        {
            ambientDay.Play();

        }

        public void PlayAmbientNight()
        {
            ambientNight.Play();
        }

        public void StopAmbientDay()
        {
            ambientDay.Stop();
        }

        public void StopAmbientNight()
        {
            ambientNight.Stop();

        }
    }
}