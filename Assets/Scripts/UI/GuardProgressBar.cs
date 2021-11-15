using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardProgressBar : MonoBehaviour
{
    [SerializeField] private Guard guard;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image sliderImage;

    public float Value;

    private bool _canIncrease = true;
    private bool _canDecrease = true;

    private float _incDecDelay = 0.2f; // increase-decrease delay

    void Start()
    {
        backgroundImage.enabled = false;
        sliderImage.enabled = false;
        Value = 0.0f;
        sliderImage.fillAmount = Value;
    }

    void Update()
    {

    }

    public void SetPorgressBarValue(float amount)
    {
        if(amount >= 1.0f)
        {
            Value = 1.0f;
            sliderImage.fillAmount = 1.0f;
            guard.OnProgressBarFilled();
        }
        else
        {
            Value = amount;
            sliderImage.fillAmount = amount;
        }
    }

    public void IncreaseProgress(float amount)
    {
        if (_canIncrease)
        {
            _canIncrease = false;

            if (Value == 0.0f)
            {
                backgroundImage.enabled = true;
                sliderImage.enabled = true;
            }

            Value = Value + amount / 10.0f;

            sliderImage.fillAmount = Value;

            if(Value >= 1.0f)
            {
                StopAllCoroutines();

                _canIncrease = true;
                Value = 1.0f;
                sliderImage.fillAmount = 1.0f;

                guard.OnProgressBarFilled(); // Call ProgressBar Filled
                return;
            }
            StartCoroutine(WaitForSmoothIncrease(_incDecDelay));
        }   
    }

    public void DecreaseProgress(float amount)
    {
        if (_canDecrease)
        {
            _canDecrease = false;
            if (Value <= 0.0001f)
            {
                StopAllCoroutines();

                backgroundImage.enabled = false;
                sliderImage.enabled = false;
                Value = 0.0f;
                _canDecrease = true;
                sliderImage.fillAmount = 0.0f;

                guard.OnProgressBarReset(); // Call ProgressBar Resetted
                return;
            }

            Value = Value - amount / 10.0f;

            sliderImage.fillAmount = Value;

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
