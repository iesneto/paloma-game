using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSaucerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource idle;
    [SerializeField] private AudioSource reward;
    [SerializeField] private AudioSource ray;
    [SerializeField] private AudioSource grab;
    [SerializeField] private float maxPitch;
    [SerializeField] private float currentPitch;
    [SerializeField] private float pitchStep;
    private bool moving;
    private bool rayPlay;

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
        for(float i = currentPitch; i <= maxPitch; i += pitchStep)
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

    
}
