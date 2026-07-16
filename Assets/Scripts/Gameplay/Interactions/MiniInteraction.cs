using System;
using UnityEngine;

public class MiniInteraction : MonoBehaviour
{
    public event Action OnInteractionCompleted;

    protected void InvokeOnInteractionCompleted()
    {
        OnInteractionCompleted?.Invoke();
    }

    public virtual void ResetInteraction()
    {

    }

    public virtual void InitializeInteraction()
    {

    }
}
