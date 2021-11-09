using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExclamationMark : MonoBehaviour
{
    private Image _exclamationImage;

    private bool _canShowMark = true; 

    private void Awake()
    {
        _exclamationImage = GetComponent<Image>();
    }

    private void Start()
    {
        _exclamationImage.enabled = false;
    }

    public void ShowExclamationMark()
    {
        if (_canShowMark)
        {
            _canShowMark = false;
            _exclamationImage.enabled = true;
            StartCoroutine(HideMarkAfterDelay(1.0f));
        }  
    }

    public void ResetExclamationMark()
    {
        _canShowMark = true;
    }

    private IEnumerator HideMarkAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _exclamationImage.enabled = false;
    }

}
