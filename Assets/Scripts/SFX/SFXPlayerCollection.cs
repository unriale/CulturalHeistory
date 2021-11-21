using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayerCollection : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] walkCycleClips;
    [SerializeField] private AudioClip[] voiceActingClips;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource walkCycleAudioSource;
    [SerializeField] private AudioSource voiceActingSource;

    private void Awake()
    {
        voiceActingSource.playOnAwake = false;
        walkCycleAudioSource.playOnAwake = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
