using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardProgressBar : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image sliderImage;

    private float _value;
    private bool _canIncrease = true;
    private bool _canDecrease = true;

    private float _incDecDelay = 0.2f; // increase-decrease delay

    void Start()
    {
        backgroundImage.enabled = false;
        sliderImage.enabled = false;
        _value = 0.0f;
        sliderImage.fillAmount = _value;
    }

    void Update()
    {

    }

    private void IncreaseProgress(float amount)
    {
        if (_canIncrease)
        {
            _canIncrease = false;

            if (_value == 0.0f)
            {
                backgroundImage.enabled = true;
                sliderImage.enabled = true;
            }

            float newVal = _value + amount / 10.0f;
            _value = Mathf.Clamp(newVal, 0.0f, 1.0f);

            sliderImage.fillAmount = _value;

            StartCoroutine(WaitForSmoothIncrease(_incDecDelay));
        }   
    }

    private void DecreaseProgress(float amount)
    {
        if (_canDecrease)
        {
            _canDecrease = false;
            if (_value == 0.0f)
            {
                backgroundImage.enabled = false;
                sliderImage.enabled = false;

                return;
            }

            float newVal = _value - amount / 10.0f;
            _value = Mathf.Clamp(newVal, 0.0f, 1.0f);

            sliderImage.fillAmount = _value;

            StartCoroutine(WaitForSmoothDecrease(_incDecDelay));
        }
    }

    private IEnumerator WaitForSmoothIncrease(float delay)
    {
        yield return new WaitForSeconds(delay);
        _canIncrease = true;
    }

    private IEnumerator WaitForSmoothDecrease(float delay)
    {
        yield return new WaitForSeconds(delay);
        _canDecrease = true;
    }
}
