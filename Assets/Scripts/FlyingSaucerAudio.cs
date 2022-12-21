using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class FlyingSaucerAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource idle;
        [SerializeField] private AudioSource reward;
        [SerializeField] private AudioSource ray;
        [SerializeField] private AudioSource grab;
        [SerializeField] private AudioSource shock;
        [SerializeField] private float maxPitch;
        [SerializeField] private float currentPitch;
        [SerializeField] private float pitchStep;
        private bool moving;
        private bool rayPlay;
        private bool stageMuted;

        private void Awake()
        {
            if (GameControl.Instance == null) return;
            if (!GameControl.Instance.playerData.sfx) MuteSFX();
            StartFlyingSaucer();
        }

        private void OnEnable()
        {
            Audio.MuteSFX += MuteSFX;
            Audio.UnmuteSFX += UnMuteSFX;
        }

        private void OnDisable()
        {
            Audio.MuteSFX -= MuteSFX;
            Audio.UnmuteSFX -= UnMuteSFX;
        }

        public void StageMuteFlyingSaucer()
        {
            stageMuted = true;
            idle.mute = true;
            reward.mute = true;
            ray.mute = true;
            grab.mute = true;
            shock.mute = true;
        }

        private void MuteSFX()
        {

            idle.mute = true;
            reward.mute = true;
            ray.mute = true;
            grab.mute = true;
            shock.mute = true;
        }

        private void UnMuteSFX()
        {
            if (stageMuted) return;

            idle.mute = false;
            reward.mute = false;
            ray.mute = false;
            grab.mute = false;
            shock.mute = false;
        }

        public void StartFlyingSaucer()
        {
            idle.Play();
            currentPitch = 1;
        }
        public void PlayMove()
        {
            //move.Play();
            moving = true;
            StartCoroutine("AdvancePitch");
        }

        public void StopMove()
        {
            //move.Stop();
            moving = false;
            StartCoroutine("RewindPitch");

        }
        IEnumerator AdvancePitch()
        {
            for (float i = currentPitch; i <= maxPitch; i += pitchStep)
            {
                idle.pitch = currentPitch = i;

                yield return null;
                if (!moving) break;
            }
        }


        IEnumerator RewindPitch()
        {
            for (float i = currentPitch; i >= 1; i -= pitchStep)
            {
                idle.pitch = currentPitch = i;
                yield return null;
                if (moving) break;
            }
        }

        public void PlayRay()
        {
            rayPlay = true;
            ray.volume = 1;
            ray.Play();

        }

        public void StopRay()
        {
            rayPlay = false;

            StartCoroutine("StopingRay");

        }

        IEnumerator StopingRay()
        {

            while (!rayPlay && ray.volume >= 0.1)
            {
                yield return null;
                ray.volume -= 0.01f;
            }

        }

        public void PlayReward()
        {
            reward.Play();
        }

        public void PlayGrab()
        {
            grab.Play();
        }

        public void PlayShocked()
        {
            shock.Play();
        }

    }
}