using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardProgressBar : MonoBehaviour
{
    #region Events
    public static event Action ProgressBarResetted;
    public static event Action ProgressBarFilled;
    #endregion

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

    private void OnEnable()
    {
        IncreasingAlert.IncreaseAlertValue += OnIncreaseAlertValue;
        DecreasingAlert.DecreaseAlertValue += OnDecreaseAlertValue;
    }

    private void OnDisable()
    {
        IncreasingAlert.IncreaseAlertValue -= OnIncreaseAlertValue;
        DecreasingAlert.DecreaseAlertValue -= OnDecreaseAlertValue;
    }

    private void OnIncreaseAlertValue(float amount)
    {
        IncreaseProgress(amount);
    }

    private void OnDecreaseAlertValue(float amount)
    {
        DecreaseProgress(amount);
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

            _value = _value + amount / 10.0f;

            sliderImage.fillAmount = _value;

            if(_value >= 1.0f)
            {
                StopAllCoroutines();

                _canIncrease = true;
                _value = 1.0f;
                sliderImage.fillAmount = 1.0f;

                ProgressBarFilled?.Invoke();
                return;
            }
            StartCoroutine(WaitForSmoothIncrease(_incDecDelay));
        }   
    }

    private void DecreaseProgress(float amount)
    {
        if (_canDecrease)
        {
            _canDecrease = false;
            if (_value <= 0.0001f)
            {
                StopAllCoroutines();

                backgroundImage.enabled = false;
                sliderImage.enabled = false;
                _value = 0.0f;
                _canDecrease = true;
                sliderImage.fillAmount = 0.0f;

                ProgressBarResetted?.Invoke(); // Invoke Event for Guard
                return;
            }

            _value = _value - amount / 10.0f;

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
