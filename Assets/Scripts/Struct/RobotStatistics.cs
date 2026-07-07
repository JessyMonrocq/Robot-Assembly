using System;
using UnityEngine;

[Serializable]
public struct RobotStatistics
{
    [Header("Robot Part Statistics")]
    public float Armor;
    public float Mobility;
    public float Strength;
    public float Computing;
    public float Energy;
    public float Weight;
}
