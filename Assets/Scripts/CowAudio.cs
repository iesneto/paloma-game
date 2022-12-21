using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class CowAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource moo;
        [SerializeField] private AudioSource bell;
        [SerializeField] private AudioClip[] mooClips;

        public void SetWalkPitch()
        {

        }

        private void Awake()
        {
            if (!GameControl.Instance.playerData.sfx) MuteSFX();
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

        private void MuteSFX()
        {
            moo.mute = true;
            bell.mute = true;
        }

        private void UnMuteSFX()
        {
            moo.mute = false;
            bell.mute = false;
        }

        public void Moo()
        {
            moo.clip = mooClips[Random.Range(0, mooClips.Length)];
            moo.Play();
        }

        public void Walk()
        {
            bell.Play();
        }

        public void StopWalk()
        {
            bell.Stop();
        }
    }
}