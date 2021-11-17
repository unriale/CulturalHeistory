using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour
{
    [SerializeField] float fadeOutTime;
    [SerializeField] float fadeInTime;

    private CanvasGroup canvasGroup;
    private Coroutine currentActiveFade;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public bool HasFadedIn()
    {
        return canvasGroup.alpha == 0;
    }

    /// 1s -> alpha 0 to 1, frame: deltaTime, num of frames = time/deltaTime, 1/0.1 = 10 frames
    /// 1 / (time/deltatime) = (deltatime / time) -> alpha val per frame 
    public IEnumerator FadeOut()
    {
        if(currentActiveFade != null)
        {
            StopCoroutine(currentActiveFade);
        }
        currentActiveFade = StartCoroutine(FadeOutRoutine());
        yield return currentActiveFade;
    }

    private IEnumerator FadeOutRoutine()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / fadeOutTime;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        if (currentActiveFade != null)
        {
            StopCoroutine(currentActiveFade);
        }
        currentActiveFade = StartCoroutine(FadeInRoutine());
        yield return currentActiveFade;
    }

    private IEnumerator FadeInRoutine()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeInTime;
            yield return null;
        }
    }

    public void FadeOutImmediate()
    {
        canvasGroup.alpha = 1;
    }
}
