using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayerCollection : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] walkCycleClips;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource walkCycleAudioSource;

    private bool isWalking = false;
    private bool isRunning = false;

    private void Awake()
    { 
        walkCycleAudioSource.playOnAwake = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayWalkCycle()
    {
        if (isRunning && walkCycleAudioSource.isPlaying)
        {
            walkCycleAudioSource.Stop();
        }
        if (!walkCycleAudioSource.isPlaying)
        {
            isRunning = false;
            isWalking = true;
            int randIndex = Random.Range(0, 8); // WalkCycle indecies
            walkCycleAudioSource.clip = walkCycleClips[randIndex];
            walkCycleAudioSource.Play();
        }
    }

    public void PlayRunCycle()
    {
        if (isWalking && walkCycleAudioSource.isPlaying)
        {
            walkCycleAudioSource.Stop();
        }
        if (!walkCycleAudioSource.isPlaying)
        {
            isRunning = true;
            isWalking = false;
            int randIndex = Random.Range(8, 16); // RunCycle indecies
            walkCycleAudioSource.clip = walkCycleClips[randIndex];
            walkCycleAudioSource.Play();
        }
    }
}
