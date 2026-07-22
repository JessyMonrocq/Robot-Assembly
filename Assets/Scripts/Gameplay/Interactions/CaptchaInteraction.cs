using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

[Serializable]
public struct CaptchaImage
{
    public Sprite sprite;
    public bool isTarget;
}

[Serializable]
public struct CaptchaPrompt
{
    public string name;
    public CaptchaImage[] captchaImages;
}

public class CaptchaInteraction : MiniInteraction
{
    [SerializeField] private TextMeshProUGUI captchaPromptName;
    [SerializeField] private CaptchaToggle[] captchaToggles;
    [SerializeField] private CanvasGroup screenCG;
    [SerializeField] private CaptchaPrompt[] captchaPrompts;

    private Dictionary<CaptchaToggle, bool> captchaToggleDict;

    private void Awake()
    {
        captchaToggleDict = new Dictionary<CaptchaToggle, bool>();
    }

    private void Start()
    {
        ResetInteraction();
        InitializeInteraction();
    }

    public override void ResetInteraction()
    {
        captchaToggleDict.Clear();

        foreach (CaptchaToggle ct in captchaToggles)
        {
            ct.SetToggle(null, false);
        }

        screenCG.alpha = 0f;
        screenCG.interactable = false;
    }

    public override void InitializeInteraction()
    {
        int randomPrompt = UnityEngine.Random.Range(0, captchaPrompts.Length);

        captchaPromptName.text = captchaPrompts[randomPrompt].name;

        foreach (CaptchaToggle ct in captchaToggles)
        {
            int randomImage = UnityEngine.Random.Range(0, captchaPrompts[randomPrompt].captchaImages.Length);
            captchaToggleDict.Add(ct, captchaPrompts[randomPrompt].captchaImages[randomImage].isTarget);
            ct.SetToggle(captchaPrompts[randomPrompt].captchaImages[randomImage].sprite, false);
        }

        screenCG.alpha = 1f;
        screenCG.interactable = true;
    }

    public void VerifyCaptchaInteraction()
    {
        bool completed = true;
        foreach (CaptchaToggle ct in captchaToggles)
        {
            if (ct.Toggle.isOn != captchaToggleDict[ct])
            {
                completed = false;
                break;
            }
        }

        if (completed)
        {
            InvokeOnInteractionCompleted();
        } else
        {
            Debug.Log("Error");
        }
    }
}
