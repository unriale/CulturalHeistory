using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] float fadeOutTime;
    [SerializeField] float fadeInTime;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// 1s -> alpha 0 to 1, frame: deltaTime, num of frames = time/deltaTime, 1/0.1 = 10 frames
    /// 1 / (time/deltatime) = (deltatime / time) -> alpha val per frame 
    public IEnumerator FadeOut()
    {
        print("In fade out");
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / fadeOutTime;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        print("In fade in");
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
