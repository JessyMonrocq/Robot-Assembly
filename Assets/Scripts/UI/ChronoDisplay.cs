using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChronoDisplay : MonoBehaviour
{
    #region Inspector Fields
    public event Action OnChronoEnded;

    [SerializeField] private Slider chronoSlider;
    [SerializeField] private TextMeshProUGUI chronoText;
    [SerializeField] private CanvasGroup chronoCanvasGroup;

    public bool Paused
    {
        set { paused = value; }
    }

    private ChronoFormat currentChrono;
    private ChronoFormat startChrono;

    private float timer;
    private bool paused;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        ResetChrono();
    }

    private void Update()
    {
        if (paused)
        {
            return;
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                timer = 0f;
            }

            currentChrono.SetTimeFromFloat(timer);
            chronoText.text = currentChrono.ToString();

            float startSeconds = startChrono.GetTimeInSeconds();
            chronoSlider.value = 1 - (startSeconds - timer) / startSeconds;
        }
        else
        {
            chronoText.text = "00\"00\'";
            chronoSlider.value = 0f;

            OnChronoEnded?.Invoke();
            paused = true;
        }
    }
    #endregion

    #region Public Methods
    public void ResetChrono()
    {
        paused = true;
        timer = 0;
        chronoSlider.value = 1f;
        chronoText.text = "00\"00\'";
        chronoCanvasGroup.alpha = 1f;
    }

    public void InitializeChrono(ChronoFormat chrono, float delayTime)
    {
        if (chrono.GetTimeInSeconds() <= 0f)
        {
            chronoCanvasGroup.alpha = 0f;
            paused = true;
            return;
        }

        startChrono = chrono;
        currentChrono = chrono;
        chronoCanvasGroup.alpha = 1f;
        timer = startChrono.GetTimeInSeconds();
        chronoText.text = startChrono.ToString();
        currentChrono.SetTimeFromFloat(timer);
        StartCoroutine(ChronoDelayCoroutine(delayTime));
    }
    #endregion

    #region Coroutine Methods
    private IEnumerator ChronoDelayCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        paused = false;
    }
    #endregion
}
