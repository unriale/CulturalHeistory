using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXCollection : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] SFXs;
    [SerializeField] private AudioClip[] walkCycleClips;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource walkCycleAudioSource;

    private Coroutine walkCoroutine;

    private void Awake()
    {
        walkCycleAudioSource.playOnAwake = false;
        SFXAudioSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayWalkCycle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int index)
    {
        SFXAudioSource.clip = SFXs[index];
        SFXAudioSource.Play();
    }

    public void PlayWalkCycle()
    {
        walkCoroutine = StartCoroutine(PlayWalkCycleCoroutine());
    }

    public void StopWalkCycle()
    {
        StopCoroutine(walkCoroutine);
    }

    private IEnumerator PlayWalkCycleCoroutine()
    {
        while (true)
        {
            int randWalkClipIndex = Random.Range(0, walkCycleClips.Length);
            walkCycleAudioSource.clip = walkCycleClips[randWalkClipIndex];
            walkCycleAudioSource.Play();
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator PlayAfetrDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SFXAudioSource.Play();
    }

}
