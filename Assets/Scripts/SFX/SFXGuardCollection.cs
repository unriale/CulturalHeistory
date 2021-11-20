using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXGuardCollection : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] SFXs;
    [SerializeField] private AudioClip[] walkCycleClips;
    [SerializeField] private AudioClip[] voiceActingClips;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource walkCycleAudioSource;
    [SerializeField] private AudioSource voiceActingSource;
    // VOICE ACTING INDICES  (indeces included):
    /* 
     * 0 - 2: Hear noise voices
     * 3 - 5: Random idle chatting
     */

    // Coroutine references
    private Coroutine walkCoroutine;
    private Coroutine chatCoroutine;

    private void Awake()
    {
        voiceActingSource.playOnAwake = false;
        walkCycleAudioSource.playOnAwake = false;
        SFXAudioSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayWalkCycle();
        PlayChatCycle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region public region
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
    
    public void PlayChatCycle()
    {
        chatCoroutine = StartCoroutine(PlayChattingCycleCoroutine());
    }

    public void StopChatCycle()
    {
        StopCoroutine(chatCoroutine);
    }

    public void PlayHearNoiseVoice()
    {
        int randIndex = Random.Range(0, 3); // 0 - 2: hear noise voices
        voiceActingSource.clip = voiceActingClips[randIndex];
        voiceActingSource.Play();
    }

    public void StopAllSFXCoroutines()
    {
        StopCoroutine(walkCoroutine);
        StopCoroutine(chatCoroutine);
    }
    #endregion

    #region Coroutine
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

    private IEnumerator PlayChattingCycleCoroutine()
    {
        // Random delay for the beginning of the chat cycle
        float randomDelay = Random.Range(4.0f, 30.0f);
        yield return new WaitForSeconds(randomDelay);

        while (true) 
        {
            if (!voiceActingSource.isPlaying)
            {
                Debug.Log("[SFXCollection PlayChattingCycleCoroutine]: chat voice invoked");
                voiceActingSource.spatialBlend = 1.0f;
                int randChatClipIndex = Random.Range(3, 6);// 3 - 5: chatting voices
                voiceActingSource.clip = voiceActingClips[randChatClipIndex];
                voiceActingSource.Play();
                yield return new WaitForSeconds(2.0f);
                voiceActingSource.spatialBlend = 0.0f;
            }

            // Random wait time for the next chat voice
            float randomTime = Random.Range(4.0f, 30.0f);
            yield return new WaitForSeconds(randomTime);
        }
    }
    #endregion

}
