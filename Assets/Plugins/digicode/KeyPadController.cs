using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class KeyPadController : MonoBehaviour
{
    public List<char> truePassword = new List<char>();
    private List<char> inputEntered = new List<char>();

    [SerializeField] private TextMeshProUGUI inputTextField;
    [SerializeField] private TextMeshProUGUI placeholderTextField;
    [SerializeField] private float resetTime = 2f;

    [SerializeField] private string successText;
    [SerializeField] private string errorText;
    [SerializeField] private string defaultText;

    [SerializeField] private GameObject letterFolder;
    [SerializeField] private GameObject numberFolder;
    [SerializeField] private GameObject AAA;
    [SerializeField] private GameObject num123;

    private bool codeCompleted = false;

    [Space(20)]
    public UnityEvent onCorrectpassword;

    public void ResetKeypad()
    {
        inputEntered.Clear();
        StopAllCoroutines();

        foreach (Button button in numberFolder.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }

        placeholderTextField.text = "____";
        inputTextField.text = null;
    }

    public void InitializeKeypad(string newPassword)
    {
        foreach (char c in newPassword)
        {
            truePassword.Add(c);
        }

        placeholderTextField.text = newPassword;
    }

    public void UserEntryLetter(string added)
    {
        if (inputEntered.Count >= truePassword.Count || codeCompleted)
        {
            return;
        }

        inputEntered.Add(added[0]);
        UpdateDisplay();

        if (inputEntered.Count >= truePassword.Count)
        {
            checkPassword();
        }
    }

    public void UserEntryInt(int added)
    {
        UserEntryLetter(added.ToString());
    }

    public void DeleteEntry()
    {
        if (inputEntered.Count <= 0 || codeCompleted)
        {
            return;
        }

        inputEntered.RemoveAt(inputEntered.Count - 1);
        UpdateDisplay();
    }


    private void checkPassword()
    {
        for (int i = 0; i < truePassword.Count; i++)
        {
            if (inputEntered[i] != truePassword[i])
            {
                IncorrectPassword();
                return;
            }
        }
        CorrectPassword();
    }

    private void CorrectPassword()
    {
        onCorrectpassword.Invoke();
        //StartCoroutine(ResetKeycode());
        inputTextField.text = successText;
        codeCompleted = true;
    }

    private void IncorrectPassword()
    {
        inputTextField.text = errorText;
        StartCoroutine(ResetKeycode());
    }

    private IEnumerator ResetKeycode()
    {
        yield return new WaitForSeconds(resetTime);
        inputEntered.Clear();
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (inputEntered.Count > 0)
        {
            placeholderTextField.text = null;
        }
        else
        {
            for (int i = 0; i < truePassword.Count; i++)
            {
                placeholderTextField.text += truePassword[i];
            }
        }

        inputTextField.text = null;
        for (int i = 0; i < inputEntered.Count; i++)
        {
            inputTextField.text += inputEntered[i];
        }
    }

    public void switchEntrys()
    {
        letterFolder.SetActive(numberFolder.activeSelf);
        numberFolder.SetActive(!numberFolder.activeSelf);
        AAA.SetActive(!AAA.activeSelf);
        num123.SetActive(!num123.activeSelf);
    }
}
