using System;
using UnityEngine;

[Serializable]
public struct SatisfactionLevel
{
    public enum SatisfactionDegree
    {
        Unsatisfied,
        Poor,
        Average,
        Good,
        Perfect
    }

    public SatisfactionDegree satisfactionDegree;
    public Sprite satisfactionIcon;
    public Color iconColor;
}
